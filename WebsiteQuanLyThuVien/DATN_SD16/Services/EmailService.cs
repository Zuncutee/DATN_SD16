using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Email
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<EmailLog> _emailLogRepository;
        private readonly IRepository<SystemSetting> _systemSettingRepository;
        private readonly LibraryDbContext _context;

        public EmailService(
            IConfiguration configuration,
            IRepository<EmailLog> emailLogRepository,
            IRepository<SystemSetting> systemSettingRepository,
            LibraryDbContext context)
        {
            _configuration = configuration;
            _emailLogRepository = emailLogRepository;
            _systemSettingRepository = systemSettingRepository;
            _context = context;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, string emailType)
        {
            try
            {
                // Lấy cấu hình email từ appsettings.json
                var smtpServer = _configuration["Email:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["Email:Username"];
                var smtpPassword = _configuration["Email:Password"];
                var fromEmail = _configuration["Email:FromEmail"] ?? smtpUsername;
                var fromName = _configuration["Email:FromName"] ?? "Thư viện";

                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    // Nếu chưa cấu hình email, chỉ log vào database (cho development)
                    return false;
                }

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(fromEmail, fromName);
                        message.To.Add(new MailAddress(to));
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        await client.SendMailAsync(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw exception
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmailWithLogAsync(int userId, string to, string subject, string body, string emailType)
        {
            var emailLog = new EmailLog
            {
                UserId = userId,
                EmailTo = to,
                EmailSubject = subject,
                EmailBody = body,
                EmailType = emailType,
                IsSent = false,
                CreatedAt = DateTime.Now
            };

            try
            {
                emailLog = await _emailLogRepository.AddAsync(emailLog);
                
                var sent = await SendEmailAsync(to, subject, body, emailType);
                
                emailLog.IsSent = sent;
                emailLog.SentAt = sent ? DateTime.Now : null;
                emailLog.ErrorMessage = sent ? null : "Gửi email thất bại. Kiểm tra cấu hình SMTP.";

                await _emailLogRepository.UpdateAsync(emailLog);
                
                return sent;
            }
            catch (Exception ex)
            {
                emailLog.ErrorMessage = ex.Message;
                await _emailLogRepository.UpdateAsync(emailLog);
                return false;
            }
        }

        public async Task<bool> SendReturnReminderEmailAsync(User user, IEnumerable<Borrow> borrows)
        {
            if (!borrows.Any()) return false;

            var finePerDay = await GetSystemSettingAsync("FinePerDay", 5000);
            
            var subject = "Nhắc nhở: Sách sắp đến hạn trả";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .book-item {{ background-color: white; padding: 15px; margin: 10px 0; border-left: 4px solid #4CAF50; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>📚 Thư viện - Nhắc nhở trả sách</h2>
        </div>
        <div class='content'>
            <p>Xin chào <strong>{user.FullName}</strong>,</p>
            <p>Bạn có <strong>{borrows.Count()}</strong> cuốn sách sắp đến hạn trả. Vui lòng đến thư viện để trả sách đúng hạn.</p>
            
            <h3>Danh sách sách sắp đến hạn:</h3>
";

            foreach (var borrow in borrows)
            {
                var daysLeft = (borrow.DueDate - DateTime.Now).Days;
                var bookTitle = borrow.Copy?.Book?.Title ?? "N/A";
                body += $@"
            <div class='book-item'>
                <strong>{bookTitle}</strong><br>
                Ngày mượn: {borrow.BorrowDate:dd/MM/yyyy}<br>
                Hạn trả: <strong>{borrow.DueDate:dd/MM/yyyy}</strong><br>
                Còn lại: <strong style='color: #ff9800;'>{daysLeft} ngày</strong>
            </div>";
            }

            body += $@"
            <p><strong>Lưu ý:</strong> Nếu trả quá hạn, bạn sẽ bị phạt {finePerDay:N0} VNĐ/ngày.</p>
            <p>Trân trọng,<br>Thư viện</p>
        </div>
        <div class='footer'>
            <p>Email này được gửi tự động từ hệ thống quản lý thư viện.</p>
        </div>
    </div>
</body>
</html>";

            return await SendEmailWithLogAsync(user.UserId, user.Email, subject, body, "ReturnReminder");
        }

        public async Task<bool> SendOverdueAlertEmailAsync(User user, IEnumerable<Borrow> borrows)
        {
            if (!borrows.Any()) return false;

            var finePerDay = await GetSystemSettingAsync("FinePerDay", 5000);
            
            var subject = "Cảnh báo: Sách đã quá hạn trả";
            var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #f44336; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .book-item {{ background-color: white; padding: 15px; margin: 10px 0; border-left: 4px solid #f44336; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>⚠️ Thư viện - Cảnh báo quá hạn</h2>
        </div>
        <div class='content'>
            <p>Xin chào <strong>{user.FullName}</strong>,</p>
            <p><strong style='color: #f44336;'>Bạn có {borrows.Count()} cuốn sách đã quá hạn trả!</strong></p>
            <p>Vui lòng đến thư viện ngay để trả sách và thanh toán phí phạt (nếu có).</p>
            
            <h3>Danh sách sách quá hạn:</h3>
";

            decimal totalFine = 0;
            foreach (var borrow in borrows)
            {
                var daysOverdue = (DateTime.Now - borrow.DueDate).Days;
                var fine = daysOverdue * finePerDay;
                totalFine += fine;
                var bookTitle = borrow.Copy?.Book?.Title ?? "N/A";
                body += $@"
            <div class='book-item'>
                <strong>{bookTitle}</strong><br>
                Ngày mượn: {borrow.BorrowDate:dd/MM/yyyy}<br>
                Hạn trả: {borrow.DueDate:dd/MM/yyyy}<br>
                Quá hạn: <strong style='color: #f44336;'>{daysOverdue} ngày</strong><br>
                Phí phạt: <strong style='color: #f44336;'>{fine:N0} VNĐ</strong>
            </div>";
            }

            body += $@"
            <p><strong style='color: #f44336; font-size: 18px;'>Tổng phí phạt: {totalFine:N0} VNĐ</strong></p>
            <p>Phí phạt: {finePerDay:N0} VNĐ/ngày quá hạn</p>
            <p>Vui lòng đến thư viện sớm nhất có thể để trả sách và thanh toán phí phạt.</p>
            <p>Trân trọng,<br>Thư viện</p>
        </div>
        <div class='footer'>
            <p>Email này được gửi tự động từ hệ thống quản lý thư viện.</p>
        </div>
    </div>
</body>
</html>";

            return await SendEmailWithLogAsync(user.UserId, user.Email, subject, body, "OverdueAlert");
        }

        private async Task<decimal> GetSystemSettingAsync(string key, decimal defaultValue)
        {
            var setting = await _systemSettingRepository.FirstOrDefaultAsync(s => s.SettingKey == key);
            if (setting != null && decimal.TryParse(setting.SettingValue, out var value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}

