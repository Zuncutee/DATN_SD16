using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Interface cho Notification Service
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Tạo thông báo
        /// </summary>
        Task<Notification> CreateNotificationAsync(int userId, string notificationType, string title, string message, int? relatedBorrowId = null, int? relatedReservationId = null);

        /// <summary>
        /// Lấy thông báo của user
        /// </summary>
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);

        /// <summary>
        /// Đánh dấu đã đọc
        /// </summary>
        Task MarkAsReadAsync(int notificationId);

        /// <summary>
        /// Đánh dấu tất cả đã đọc
        /// </summary>
        Task MarkAllAsReadAsync(int userId);

        /// <summary>
        /// Đếm số thông báo chưa đọc
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId);
    }
}

