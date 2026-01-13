using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DATN_SD16.Services
{
    // Service implementation cho Notification
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IBorrowRepository _borrowRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IRepository<Notification> notificationRepository,
            IBorrowRepository borrowRepository,
            IEmailService emailService,
            IRepository<SystemSetting> systemSettingRepository,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _borrowRepository = borrowRepository;
            _emailService = emailService;
            _systemSettingRepository = systemSettingRepository;
            _logger = logger;
        }

        public async Task<Notification> CreateNotificationAsync(int userId, string notificationType, string title, string message, int? relatedBorrowId = null, int? relatedReservationId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                NotificationType = notificationType,
                Title = title,
                Message = message,
                RelatedBorrowId = relatedBorrowId,
                RelatedReservationId = relatedReservationId,
                IsRead = false,
                IsEmailSent = false,
                CreatedAt = DateTime.Now
            };

            return await _notificationRepository.AddAsync(notification);
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
        {
            if (unreadOnly)
            {
                return await _notificationRepository.FindAsync(n => n.UserId == userId && !n.IsRead);
            }
            return await _notificationRepository.FindAsync(n => n.UserId == userId);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
                await _notificationRepository.UpdateAsync(notification);
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _notificationRepository.FindAsync(n => n.UserId == userId && !n.IsRead);
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }
            await _notificationRepository.UpdateRangeAsync(notifications);
            }

        public async Task ProcessDailyNotificationsAsync()
        {
            try
            {
                // Lấy cấu hình từ SystemSettings
                var reminderDaysSetting = await _systemSettingRepository.FirstOrDefaultAsync(s => s.SettingKey == "EmailReturnReminderDays");
                var overdueDaysSetting = await _systemSettingRepository.FirstOrDefaultAsync(s => s.SettingKey == "EmailOverdueAlertDays");

                int reminderDays = 2; // Mặc định 2 ngày
                int overdueDays = 0; // Mặc định 0 ngày (thông báo ngay khi quá hạn)

                if (reminderDaysSetting != null && int.TryParse(reminderDaysSetting.SettingValue, out var rd))
                {
                    reminderDays = rd;
                }

                if (overdueDaysSetting != null && int.TryParse(overdueDaysSetting.SettingValue, out var od))
                {
                    overdueDays = od;
                }

                // 1. Xử lý nhắc nhở sắp đến hạn
                await ProcessReturnRemindersAsync(reminderDays);

                // 2. Xử lý cảnh báo quá hạn
                await ProcessOverdueAlertsAsync(overdueDays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý thông báo");
            }
        }

        private async Task ProcessReturnRemindersAsync(int reminderDays)
        {
            // Lấy các sách sắp đến hạn (trong vòng reminderDays ngày)
            var borrowsNearingDue = await _borrowRepository.GetBorrowsNearingDueDateAsync(reminderDays);

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
                var recentNotification = await _notificationRepository.FirstOrDefaultAsync(n =>
                    n.UserId == userId
                    && n.NotificationType == "ReturnReminder"
                    && n.CreatedAt > DateTime.Now.AddHours(-24));

                if (recentNotification == null && !string.IsNullOrEmpty(user.Email))
                {
                    // Tạo thông báo
                    var notification = await CreateNotificationAsync(
                        userId,
                        "ReturnReminder",
                        $"Nhắc nhở: {userBorrows.Count} cuốn sách sắp đến hạn trả",
                        $"Bạn có {userBorrows.Count} cuốn sách sắp đến hạn trả trong {reminderDays} ngày tới.",
                        relatedBorrowId: userBorrows.First().BorrowId
                    );

                    // Gửi email
                    var emailSent = await _emailService.SendReturnReminderEmailAsync(user, userBorrows);

                    // Cập nhật trạng thái email
                    notification.IsEmailSent = emailSent;
                    notification.EmailSentAt = emailSent ? DateTime.Now : null;
                    await _notificationRepository.UpdateAsync(notification);

                    _logger.LogInformation($"Đã gửi nhắc nhở trả sách cho user {userId}");
                }
            }
        }

        private async Task ProcessOverdueAlertsAsync(int overdueDays)
        {
            // Lấy các sách quá hạn
            var overdueBorrows = await _borrowRepository.GetOverdueBorrowsAsync();

            // Chỉ xử lý những sách quá hạn >= overdueDays
            var thresholdTime = DateTime.Now.AddDays(-overdueDays);
            var borrowsToAlert = overdueBorrows
                .Where(b => b.DueDate <= thresholdTime)
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
                var recentNotification = await _notificationRepository.FirstOrDefaultAsync(n =>
                    n.UserId == userId
                    && n.NotificationType == "OverdueAlert"
                    && n.CreatedAt > DateTime.Now.AddHours(-24));

                if (recentNotification == null && !string.IsNullOrEmpty(user.Email))
                {
                    // Tạo thông báo
                    var notification = await CreateNotificationAsync(
                        userId,
                        "OverdueAlert",
                        $"Cảnh báo: {userBorrows.Count} cuốn sách đã quá hạn",
                        $"Bạn có {userBorrows.Count} cuốn sách đã quá hạn trả. Vui lòng đến thư viện ngay.",
                        relatedBorrowId: userBorrows.First().BorrowId
                    );

                    // Gửi email
                    var emailSent = await _emailService.SendOverdueAlertEmailAsync(user, userBorrows);

                    // Cập nhật trạng thái email
                    notification.IsEmailSent = emailSent;
                    notification.EmailSentAt = emailSent ? DateTime.Now : null;
                    await _notificationRepository.UpdateAsync(notification);

                    _logger.LogInformation($"Đã gửi cảnh báo quá hạn cho user {userId}");
                }
            }
        }


        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string notificationType, bool includeRead = true)
        {
            if (includeRead)
            {
                return await _notificationRepository.FindAsync(n => n.NotificationType == notificationType);
            }
            return await _notificationRepository.FindAsync(n => n.NotificationType == notificationType && !n.IsRead);
        }
    }
}

