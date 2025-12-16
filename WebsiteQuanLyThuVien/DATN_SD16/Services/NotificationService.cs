using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Notification
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Notification> _notificationRepository;

        public NotificationService(IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
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

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}

