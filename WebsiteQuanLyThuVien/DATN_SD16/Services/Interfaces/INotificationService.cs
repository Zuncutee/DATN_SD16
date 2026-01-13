using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    // Interface cho Notification Service
    public interface INotificationService
    {
        // Tạo thông báo
        Task<Notification> CreateNotificationAsync(int userId, string notificationType, string title, string message, int? relatedBorrowId = null, int? relatedReservationId = null);

        // Lấy thông báo của user
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);

        // Đánh dấu đã đọc
        Task MarkAsReadAsync(int notificationId);

        // Đánh dấu tất cả đã đọc
        Task MarkAllAsReadAsync(int userId);

        // Đếm số thông báo chưa đọc
        Task<int> GetUnreadCountAsync(int userId);

        // Lấy tất cả thông báo theo type (cho admin)
        Task<IEnumerable<Notification>> GetNotificationsByTypeAsync(string notificationType, bool includeRead = true);

        // Xử lý thông báo định kỳ (Daily)
        Task ProcessDailyNotificationsAsync();
    }
}

