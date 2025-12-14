using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Background service để gửi thông báo email tự động
    /// </summary>
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Kiểm tra mỗi giờ

        public NotificationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationBackgroundService đã khởi động.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessNotificationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xử lý thông báo tự động");
                }

                // Chờ đến lần kiểm tra tiếp theo
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessNotificationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var borrowRepository = scope.ServiceProvider.GetRequiredService<IBorrowRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var systemSettingRepository = scope.ServiceProvider.GetRequiredService<IRepository<SystemSetting>>();
            var notificationRepository = scope.ServiceProvider.GetRequiredService<IRepository<Notification>>();

            try
            {
                // Lấy cấu hình từ SystemSettings
                var reminderDaysSetting = await systemSettingRepository.FirstOrDefaultAsync(s => s.SettingKey == "EmailReturnReminderDays");
                var overdueDaysSetting = await systemSettingRepository.FirstOrDefaultAsync(s => s.SettingKey == "EmailOverdueAlertDays");

                int reminderDays = 2; // Mặc định 2 ngày
                int overdueDays = 1; // Mặc định 1 ngày

                if (reminderDaysSetting != null && int.TryParse(reminderDaysSetting.SettingValue, out var rd))
                {
                    reminderDays = rd;
                }

                if (overdueDaysSetting != null && int.TryParse(overdueDaysSetting.SettingValue, out var od))
                {
                    overdueDays = od;
                }

                // 1. Xử lý nhắc nhở sắp đến hạn
                await ProcessReturnRemindersAsync(borrowRepository, emailService, notificationService, notificationRepository, reminderDays);

                // 2. Xử lý cảnh báo quá hạn
                await ProcessOverdueAlertsAsync(borrowRepository, emailService, notificationService, notificationRepository, overdueDays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý thông báo");
            }
        }

        private async Task ProcessReturnRemindersAsync(
            IBorrowRepository borrowRepository,
            IEmailService emailService,
            INotificationService notificationService,
            IRepository<Notification> notificationRepository,
            int reminderDays)
        {
            // Lấy các sách sắp đến hạn (trong vòng reminderDays ngày)
            var borrowsNearingDue = await borrowRepository.GetBorrowsNearingDueDateAsync(reminderDays);

            // Nhóm theo UserId
            var borrowsByUser = borrowsNearingDue
                .GroupBy(b => b.UserId)
                .ToList();

            foreach (var userGroup in borrowsByUser)
            {
                var userId = userGroup.Key;
                var userBorrows = userGroup.ToList();
                var user = userBorrows.First().User;

                // Kiểm tra xem đã gửi thông báo chưa (trong 24 giờ qua)
                var recentNotification = await notificationRepository.FirstOrDefaultAsync(n =>
                    n.UserId == userId
                    && n.NotificationType == "ReturnReminder"
                    && n.CreatedAt > DateTime.Now.AddHours(-24));

                if (recentNotification == null && !string.IsNullOrEmpty(user.Email))
                {
                    // Tạo thông báo
                    var notification = await notificationService.CreateNotificationAsync(
                        userId,
                        "ReturnReminder",
                        $"Nhắc nhở: {userBorrows.Count} cuốn sách sắp đến hạn trả",
                        $"Bạn có {userBorrows.Count} cuốn sách sắp đến hạn trả trong {reminderDays} ngày tới.",
                        relatedBorrowId: userBorrows.First().BorrowId
                    );

                    // Gửi email
                    var emailSent = await emailService.SendReturnReminderEmailAsync(user, userBorrows);

                    // Cập nhật trạng thái email
                    notification.IsEmailSent = emailSent;
                    notification.EmailSentAt = emailSent ? DateTime.Now : null;
                    await notificationRepository.UpdateAsync(notification);

                    _logger.LogInformation($"Đã gửi nhắc nhở trả sách cho user {userId}");
                }
            }
        }

        private async Task ProcessOverdueAlertsAsync(
            IBorrowRepository borrowRepository,
            IEmailService emailService,
            INotificationService notificationService,
            IRepository<Notification> notificationRepository,
            int overdueDays)
        {
            // Lấy các sách quá hạn
            var overdueBorrows = await borrowRepository.GetOverdueBorrowsAsync();

            // Chỉ xử lý những sách quá hạn đúng overdueDays ngày
            var targetDate = DateTime.Now.AddDays(-overdueDays).Date;
            var borrowsToAlert = overdueBorrows
                .Where(b => b.DueDate.Date == targetDate)
                .ToList();

            if (!borrowsToAlert.Any()) return;

            // Nhóm theo UserId
            var borrowsByUser = borrowsToAlert
                .GroupBy(b => b.UserId)
                .ToList();

            foreach (var userGroup in borrowsByUser)
            {
                var userId = userGroup.Key;
                var userBorrows = userGroup.ToList();
                var user = userBorrows.First().User;

                // Kiểm tra xem đã gửi thông báo chưa (trong 24 giờ qua)
                var recentNotification = await notificationRepository.FirstOrDefaultAsync(n =>
                    n.UserId == userId
                    && n.NotificationType == "OverdueAlert"
                    && n.CreatedAt > DateTime.Now.AddHours(-24));

                if (recentNotification == null && !string.IsNullOrEmpty(user.Email))
                {
                    // Tạo thông báo
                    var notification = await notificationService.CreateNotificationAsync(
                        userId,
                        "OverdueAlert",
                        $"Cảnh báo: {userBorrows.Count} cuốn sách đã quá hạn",
                        $"Bạn có {userBorrows.Count} cuốn sách đã quá hạn trả. Vui lòng đến thư viện ngay.",
                        relatedBorrowId: userBorrows.First().BorrowId
                    );

                    // Gửi email
                    var emailSent = await emailService.SendOverdueAlertEmailAsync(user, userBorrows);

                    // Cập nhật trạng thái email
                    notification.IsEmailSent = emailSent;
                    notification.EmailSentAt = emailSent ? DateTime.Now : null;
                    await notificationRepository.UpdateAsync(notification);

                    _logger.LogInformation($"Đã gửi cảnh báo quá hạn cho user {userId}");
                }
            }
        }
    }
}

