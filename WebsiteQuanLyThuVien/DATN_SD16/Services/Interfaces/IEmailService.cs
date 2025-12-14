using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    /// <summary>
    /// Interface cho Email Service
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Gửi email
        /// </summary>
        Task<bool> SendEmailAsync(string to, string subject, string body, string emailType);

        /// <summary>
        /// Gửi email và lưu vào EmailLog
        /// </summary>
        Task<bool> SendEmailWithLogAsync(int userId, string to, string subject, string body, string emailType);

        /// <summary>
        /// Gửi email nhắc trả sách
        /// </summary>
        Task<bool> SendReturnReminderEmailAsync(User user, IEnumerable<Borrow> borrows);

        /// <summary>
        /// Gửi email cảnh báo quá hạn
        /// </summary>
        Task<bool> SendOverdueAlertEmailAsync(User user, IEnumerable<Borrow> borrows);
    }
}

