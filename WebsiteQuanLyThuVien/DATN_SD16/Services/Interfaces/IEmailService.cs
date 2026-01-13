using DATN_SD16.Models.Entities;

namespace DATN_SD16.Services.Interfaces
{
    // Interface cho Email Service
    public interface IEmailService
    {
        // Gửi email
        Task<bool> SendEmailAsync(string to, string subject, string body, string emailType);

        // Gửi email và lưu vào EmailLog
        Task<bool> SendEmailWithLogAsync(int userId, string to, string subject, string body, string emailType);

        // Gửi email nhắc trả sách
        Task<bool> SendReturnReminderEmailAsync(User user, IEnumerable<Borrow> borrows);

        // Gửi email cảnh báo quá hạn
        Task<bool> SendOverdueAlertEmailAsync(User user, IEnumerable<Borrow> borrows);
    }
}

