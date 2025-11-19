using Microsoft.EntityFrameworkCore;
using DATN_SD16.Models.Entities;
using System;

namespace DATN_SD16.Data
{
    /// <summary>
    /// DbContext cho hệ thống quản lý thư viện
    /// </summary>
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        // Quản lý người dùng & phân quyền
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<LibraryCard> LibraryCards { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        // Quản lý danh mục
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookLocation> BookLocations { get; set; }

        // Quản lý sách
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<BookImport> BookImports { get; set; }

        // Quản lý mượn-trả
        public DbSet<BookReservation> BookReservations { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<BorrowHistory> BorrowHistories { get; set; }

        // Quản lý sách hỏng/mất
        public DbSet<BookDamage> BookDamages { get; set; }

        // Kiểm kê sách
        public DbSet<InventoryCheck> InventoryChecks { get; set; }
        public DbSet<InventoryCheckDetail> InventoryCheckDetails { get; set; }

        // Cấu hình hệ thống
        public DbSet<SystemSetting> SystemSettings { get; set; }

        // Đánh giá sách
        public DbSet<BookReview> BookReviews { get; set; }

        // Đặt chỗ phòng đọc
        public DbSet<ReadingRoom> ReadingRooms { get; set; }
        public DbSet<ReadingRoomSeat> ReadingRoomSeats { get; set; }
        public DbSet<ReadingRoomReservation> ReadingRoomReservations { get; set; }

        // Thông báo tự động
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }

        // JWT Refresh Tokens
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ và ràng buộc

            // User - Role (Many-to-Many)
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book - Author (Many-to-Many)
            modelBuilder.Entity<BookAuthor>()
                .HasIndex(ba => new { ba.BookId, ba.AuthorId })
                .IsUnique();

            // BookCopy - Book (One-to-Many)
            modelBuilder.Entity<BookCopy>()
                .HasIndex(bc => new { bc.BookId, bc.CopyNumber })
                .IsUnique();

            // BookReview - Unique constraint
            modelBuilder.Entity<BookReview>()
                .HasIndex(br => new { br.BookId, br.UserId })
                .IsUnique();

            // BookReview - ApprovedBy relationship
            modelBuilder.Entity<BookReview>()
                .HasOne(br => br.ApprovedByUser)
                .WithMany()
                .HasForeignKey(br => br.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // LibraryCard - CreatedBy relationship
            modelBuilder.Entity<LibraryCard>()
                .HasOne(lc => lc.CreatedByUser)
                .WithMany()
                .HasForeignKey(lc => lc.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // BookReservation - ApprovedBy relationship
            modelBuilder.Entity<BookReservation>()
                .HasOne(br => br.ApprovedByUser)
                .WithMany()
                .HasForeignKey(br => br.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ReadingRoomSeat - Unique constraint
            modelBuilder.Entity<ReadingRoomSeat>()
                .HasIndex(rrs => new { rrs.RoomId, rrs.SeatNumber })
                .IsUnique();

            // InventoryCheckDetail - Unique constraint
            modelBuilder.Entity<InventoryCheckDetail>()
                .HasIndex(icd => new { icd.CheckId, icd.CopyId })
                .IsUnique();

            // Cấu hình decimal precision
            modelBuilder.Entity<Borrow>()
                .Property(b => b.FineAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Borrow>()
                .Property(b => b.FinePaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BookCopy>()
                .Property(bc => bc.PurchasePrice)
                .HasPrecision(18, 2);

            // Cấu hình default values
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<User>()
                .Property(u => u.IsLocked)
                .HasDefaultValue(false);

            modelBuilder.Entity<User>()
                .Property(u => u.FailedLoginAttempts)
                .HasDefaultValue(0);

            modelBuilder.Entity<BookCopy>()
                .Property(bc => bc.Status)
                .HasDefaultValue("Available");

            modelBuilder.Entity<BookCopy>()
                .Property(bc => bc.Condition)
                .HasDefaultValue("Good");

            modelBuilder.Entity<BookReservation>()
                .Property(br => br.Status)
                .HasDefaultValue("Pending");

            modelBuilder.Entity<Borrow>()
                .Property(b => b.Status)
                .HasDefaultValue("Borrowed");

            modelBuilder.Entity<Borrow>()
                .Property(b => b.FineAmount)
                .HasDefaultValue(0);

            modelBuilder.Entity<Borrow>()
                .Property(b => b.FinePaid)
                .HasDefaultValue(0);

            // Borrow - Librarian relationships
            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.BorrowedByUser)
                .WithMany()
                .HasForeignKey(b => b.BorrowedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.ReturnedByUser)
                .WithMany()
                .HasForeignKey(b => b.ReturnedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification relationships
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.RelatedBorrow)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.RelatedBorrowId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.RelatedReservation)
                .WithMany(r => r.Notifications)
                .HasForeignKey(n => n.RelatedReservationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed data cho Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Quản trị viên hệ thống - Toàn quyền quản lý", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Role { RoleId = 2, RoleName = "Librarian", Description = "Thủ thư - Quản lý mượn trả, kho sách", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                new Role { RoleId = 3, RoleName = "Reader", Description = "Độc giả - Mượn sách, tìm kiếm", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );

            // Seed data cho SystemSettings
            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting { SettingId = 1, SettingKey = "MaxBorrowDays", SettingValue = "14", Description = "Số ngày mượn tối đa", Category = "Borrowing", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 2, SettingKey = "MaxRenewDays", SettingValue = "7", Description = "Số ngày gia hạn tối đa", Category = "Borrowing", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 3, SettingKey = "MaxBorrowBooks", SettingValue = "5", Description = "Số sách mượn tối đa cùng lúc", Category = "Borrowing", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 4, SettingKey = "FinePerDay", SettingValue = "5000", Description = "Phí phạt mỗi ngày quá hạn (VNĐ)", Category = "Fine", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 5, SettingKey = "ReservationExpiryDays", SettingValue = "3", Description = "Số ngày hết hạn đặt sách", Category = "Reservation", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 6, SettingKey = "EmailReturnReminderDays", SettingValue = "2", Description = "Số ngày trước khi trả để gửi email nhắc", Category = "Notification", UpdatedAt = DateTime.Now },
                new SystemSetting { SettingId = 7, SettingKey = "EmailOverdueAlertDays", SettingValue = "1", Description = "Số ngày quá hạn để gửi email cảnh báo", Category = "Notification", UpdatedAt = DateTime.Now }
            );
        }
    }
}

