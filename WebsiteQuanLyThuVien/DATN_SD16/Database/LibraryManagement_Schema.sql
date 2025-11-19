/*
 Navicat Premium Dump SQL

 Source Server         : sql_server
 Source Server Type    : SQL Server
 Source Server Version : 16001000 (16.00.1000)
 Source Host           : localhost:1433
 Source Catalog        : LibraryManagement_SD16
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16001000 (16.00.1000)
 File Encoding         : 65001

 Date: 19/11/2025 10:20:51
*/


-- ----------------------------
-- Table structure for Authors
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Authors]') AND type IN ('U'))
	DROP TABLE [dbo].[Authors]
GO

CREATE TABLE [dbo].[Authors] (
  [AuthorId] int  IDENTITY(1,1) NOT NULL,
  [AuthorName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Biography] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Nationality] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Authors] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Authors
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Authors] ON
GO

SET IDENTITY_INSERT [dbo].[Authors] OFF
GO


-- ----------------------------
-- Table structure for BookAuthors
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookAuthors]') AND type IN ('U'))
	DROP TABLE [dbo].[BookAuthors]
GO

CREATE TABLE [dbo].[BookAuthors] (
  [BookAuthorId] int  IDENTITY(1,1) NOT NULL,
  [BookId] int  NOT NULL,
  [AuthorId] int  NOT NULL,
  [IsPrimary] bit DEFAULT 1 NULL
)
GO

ALTER TABLE [dbo].[BookAuthors] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookAuthors
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookAuthors] ON
GO

SET IDENTITY_INSERT [dbo].[BookAuthors] OFF
GO


-- ----------------------------
-- Table structure for BookCopies
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookCopies]') AND type IN ('U'))
	DROP TABLE [dbo].[BookCopies]
GO

CREATE TABLE [dbo].[BookCopies] (
  [CopyId] int  IDENTITY(1,1) NOT NULL,
  [BookId] int  NOT NULL,
  [CopyNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Barcode] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Available' NULL,
  [Condition] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Good' NULL,
  [PurchaseDate] date  NULL,
  [PurchasePrice] decimal(18,2)  NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookCopies] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookCopies
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookCopies] ON
GO

SET IDENTITY_INSERT [dbo].[BookCopies] OFF
GO


-- ----------------------------
-- Table structure for BookDamages
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookDamages]') AND type IN ('U'))
	DROP TABLE [dbo].[BookDamages]
GO

CREATE TABLE [dbo].[BookDamages] (
  [DamageId] int  IDENTITY(1,1) NOT NULL,
  [CopyId] int  NOT NULL,
  [DamageType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [DamageDate] date DEFAULT getdate() NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ReportedBy] int  NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Reported' NULL,
  [ProcessedBy] int  NULL,
  [ProcessedAt] datetime  NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookDamages] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookDamages
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookDamages] ON
GO

SET IDENTITY_INSERT [dbo].[BookDamages] OFF
GO


-- ----------------------------
-- Table structure for BookImports
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookImports]') AND type IN ('U'))
	DROP TABLE [dbo].[BookImports]
GO

CREATE TABLE [dbo].[BookImports] (
  [ImportId] int  IDENTITY(1,1) NOT NULL,
  [BookId] int  NOT NULL,
  [Quantity] int  NOT NULL,
  [ImportDate] date DEFAULT getdate() NOT NULL,
  [ImportedBy] int  NOT NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookImports] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookImports
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookImports] ON
GO

SET IDENTITY_INSERT [dbo].[BookImports] OFF
GO


-- ----------------------------
-- Table structure for BookLocations
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookLocations]') AND type IN ('U'))
	DROP TABLE [dbo].[BookLocations]
GO

CREATE TABLE [dbo].[BookLocations] (
  [LocationId] int  IDENTITY(1,1) NOT NULL,
  [LocationCode] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ShelfNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [RowNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookLocations] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookLocations
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookLocations] ON
GO

SET IDENTITY_INSERT [dbo].[BookLocations] OFF
GO


-- ----------------------------
-- Table structure for BookReservations
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookReservations]') AND type IN ('U'))
	DROP TABLE [dbo].[BookReservations]
GO

CREATE TABLE [dbo].[BookReservations] (
  [ReservationId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [BookId] int  NOT NULL,
  [ReservationDate] datetime DEFAULT getdate() NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Pending' NULL,
  [ExpiryDate] datetime  NULL,
  [ApprovedBy] int  NULL,
  [ApprovedAt] datetime  NULL,
  [RejectionReason] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookReservations] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookReservations
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookReservations] ON
GO

SET IDENTITY_INSERT [dbo].[BookReservations] OFF
GO


-- ----------------------------
-- Table structure for BookReviews
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BookReviews]') AND type IN ('U'))
	DROP TABLE [dbo].[BookReviews]
GO

CREATE TABLE [dbo].[BookReviews] (
  [ReviewId] int  IDENTITY(1,1) NOT NULL,
  [BookId] int  NOT NULL,
  [UserId] int  NOT NULL,
  [Rating] int  NOT NULL,
  [ReviewText] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsApproved] bit DEFAULT 0 NULL,
  [ApprovedBy] int  NULL,
  [ApprovedAt] datetime  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[BookReviews] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BookReviews
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BookReviews] ON
GO

SET IDENTITY_INSERT [dbo].[BookReviews] OFF
GO


-- ----------------------------
-- Table structure for Books
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Books]') AND type IN ('U'))
	DROP TABLE [dbo].[Books]
GO

CREATE TABLE [dbo].[Books] (
  [BookId] int  IDENTITY(1,1) NOT NULL,
  [ISBN] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Title] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CoverImage] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Language] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT N'Tiếng Việt' NULL,
  [PublicationYear] int  NULL,
  [PageCount] int  NULL,
  [CategoryId] int  NOT NULL,
  [PublisherId] int  NULL,
  [LocationId] int  NULL,
  [TotalCopies] int DEFAULT 0 NULL,
  [AvailableCopies] int DEFAULT 0 NULL,
  [BorrowedCopies] int DEFAULT 0 NULL,
  [LostCopies] int DEFAULT 0 NULL,
  [DamagedCopies] int DEFAULT 0 NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NULL,
  [CreatedBy] int  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Books] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Books
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Books] ON
GO

SET IDENTITY_INSERT [dbo].[Books] OFF
GO


-- ----------------------------
-- Table structure for BorrowHistory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BorrowHistory]') AND type IN ('U'))
	DROP TABLE [dbo].[BorrowHistory]
GO

CREATE TABLE [dbo].[BorrowHistory] (
  [HistoryId] int  IDENTITY(1,1) NOT NULL,
  [BorrowId] int  NOT NULL,
  [UserId] int  NOT NULL,
  [CopyId] int  NOT NULL,
  [Action] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ActionDate] datetime DEFAULT getdate() NOT NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[BorrowHistory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of BorrowHistory
-- ----------------------------
SET IDENTITY_INSERT [dbo].[BorrowHistory] ON
GO

SET IDENTITY_INSERT [dbo].[BorrowHistory] OFF
GO


-- ----------------------------
-- Table structure for Borrows
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Borrows]') AND type IN ('U'))
	DROP TABLE [dbo].[Borrows]
GO

CREATE TABLE [dbo].[Borrows] (
  [BorrowId] int  IDENTITY(1,1) NOT NULL,
  [BorrowNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [UserId] int  NOT NULL,
  [CopyId] int  NOT NULL,
  [ReservationId] int  NULL,
  [BorrowDate] date DEFAULT getdate() NOT NULL,
  [DueDate] date  NOT NULL,
  [ReturnDate] date  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Borrowed' NULL,
  [FineAmount] decimal(18,2) DEFAULT 0 NULL,
  [FinePaid] decimal(18,2) DEFAULT 0 NULL,
  [ConditionOnBorrow] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ConditionOnReturn] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [BorrowedBy] int  NOT NULL,
  [ReturnedBy] int  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Borrows] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Borrows
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Borrows] ON
GO

SET IDENTITY_INSERT [dbo].[Borrows] OFF
GO


-- ----------------------------
-- Table structure for Categories
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type IN ('U'))
	DROP TABLE [dbo].[Categories]
GO

CREATE TABLE [dbo].[Categories] (
  [CategoryId] int  IDENTITY(1,1) NOT NULL,
  [CategoryName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Categories] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Categories
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Categories] ON
GO

SET IDENTITY_INSERT [dbo].[Categories] OFF
GO


-- ----------------------------
-- Table structure for EmailLogs
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[EmailLogs]') AND type IN ('U'))
	DROP TABLE [dbo].[EmailLogs]
GO

CREATE TABLE [dbo].[EmailLogs] (
  [EmailLogId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [EmailTo] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EmailSubject] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EmailBody] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [EmailType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [IsSent] bit DEFAULT 0 NULL,
  [SentAt] datetime  NULL,
  [ErrorMessage] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[EmailLogs] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of EmailLogs
-- ----------------------------
SET IDENTITY_INSERT [dbo].[EmailLogs] ON
GO

SET IDENTITY_INSERT [dbo].[EmailLogs] OFF
GO


-- ----------------------------
-- Table structure for InventoryCheckDetails
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryCheckDetails]') AND type IN ('U'))
	DROP TABLE [dbo].[InventoryCheckDetails]
GO

CREATE TABLE [dbo].[InventoryCheckDetails] (
  [DetailId] int  IDENTITY(1,1) NOT NULL,
  [CheckId] int  NOT NULL,
  [CopyId] int  NOT NULL,
  [ExpectedStatus] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ActualStatus] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsMatched] bit DEFAULT 0 NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[InventoryCheckDetails] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of InventoryCheckDetails
-- ----------------------------
SET IDENTITY_INSERT [dbo].[InventoryCheckDetails] ON
GO

SET IDENTITY_INSERT [dbo].[InventoryCheckDetails] OFF
GO


-- ----------------------------
-- Table structure for InventoryChecks
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryChecks]') AND type IN ('U'))
	DROP TABLE [dbo].[InventoryChecks]
GO

CREATE TABLE [dbo].[InventoryChecks] (
  [CheckId] int  IDENTITY(1,1) NOT NULL,
  [CheckNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [CheckDate] date DEFAULT getdate() NOT NULL,
  [CheckedBy] int  NOT NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'InProgress' NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[InventoryChecks] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of InventoryChecks
-- ----------------------------
SET IDENTITY_INSERT [dbo].[InventoryChecks] ON
GO

SET IDENTITY_INSERT [dbo].[InventoryChecks] OFF
GO


-- ----------------------------
-- Table structure for LibraryCards
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[LibraryCards]') AND type IN ('U'))
	DROP TABLE [dbo].[LibraryCards]
GO

CREATE TABLE [dbo].[LibraryCards] (
  [CardId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [CardNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [IssueDate] date DEFAULT getdate() NOT NULL,
  [ExpiryDate] date  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Active' NULL,
  [CreatedBy] int  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[LibraryCards] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of LibraryCards
-- ----------------------------
SET IDENTITY_INSERT [dbo].[LibraryCards] ON
GO

SET IDENTITY_INSERT [dbo].[LibraryCards] OFF
GO


-- ----------------------------
-- Table structure for Notifications
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Notifications]') AND type IN ('U'))
	DROP TABLE [dbo].[Notifications]
GO

CREATE TABLE [dbo].[Notifications] (
  [NotificationId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [NotificationType] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Title] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Message] nvarchar(max) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [IsRead] bit DEFAULT 0 NULL,
  [IsEmailSent] bit DEFAULT 0 NULL,
  [EmailSentAt] datetime  NULL,
  [RelatedBorrowId] int  NULL,
  [RelatedReservationId] int  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [ReadAt] datetime  NULL
)
GO

ALTER TABLE [dbo].[Notifications] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Notifications
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Notifications] ON
GO

SET IDENTITY_INSERT [dbo].[Notifications] OFF
GO


-- ----------------------------
-- Table structure for PasswordResetTokens
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PasswordResetTokens]') AND type IN ('U'))
	DROP TABLE [dbo].[PasswordResetTokens]
GO

CREATE TABLE [dbo].[PasswordResetTokens] (
  [TokenId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [Token] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ExpiresAt] datetime  NOT NULL,
  [IsUsed] bit DEFAULT 0 NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[PasswordResetTokens] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of PasswordResetTokens
-- ----------------------------
SET IDENTITY_INSERT [dbo].[PasswordResetTokens] ON
GO

SET IDENTITY_INSERT [dbo].[PasswordResetTokens] OFF
GO


-- ----------------------------
-- Table structure for Publishers
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Publishers]') AND type IN ('U'))
	DROP TABLE [dbo].[Publishers]
GO

CREATE TABLE [dbo].[Publishers] (
  [PublisherId] int  IDENTITY(1,1) NOT NULL,
  [PublisherName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Address] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [PhoneNumber] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Email] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Publishers] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Publishers
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Publishers] ON
GO

SET IDENTITY_INSERT [dbo].[Publishers] OFF
GO


-- ----------------------------
-- Table structure for ReadingRoomReservations
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ReadingRoomReservations]') AND type IN ('U'))
	DROP TABLE [dbo].[ReadingRoomReservations]
GO

CREATE TABLE [dbo].[ReadingRoomReservations] (
  [ReservationId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [SeatId] int  NOT NULL,
  [ReservationDate] date  NOT NULL,
  [StartTime] time(7)  NOT NULL,
  [EndTime] time(7)  NOT NULL,
  [CheckInTime] datetime  NULL,
  [CheckOutTime] datetime  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Reserved' NULL,
  [Notes] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[ReadingRoomReservations] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ReadingRoomReservations
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ReadingRoomReservations] ON
GO

SET IDENTITY_INSERT [dbo].[ReadingRoomReservations] OFF
GO


-- ----------------------------
-- Table structure for ReadingRooms
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ReadingRooms]') AND type IN ('U'))
	DROP TABLE [dbo].[ReadingRooms]
GO

CREATE TABLE [dbo].[ReadingRooms] (
  [RoomId] int  IDENTITY(1,1) NOT NULL,
  [RoomName] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [RoomCode] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Capacity] int  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsActive] bit DEFAULT 1 NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[ReadingRooms] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ReadingRooms
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ReadingRooms] ON
GO

SET IDENTITY_INSERT [dbo].[ReadingRooms] OFF
GO


-- ----------------------------
-- Table structure for ReadingRoomSeats
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[ReadingRoomSeats]') AND type IN ('U'))
	DROP TABLE [dbo].[ReadingRoomSeats]
GO

CREATE TABLE [dbo].[ReadingRoomSeats] (
  [SeatId] int  IDENTITY(1,1) NOT NULL,
  [RoomId] int  NOT NULL,
  [SeatNumber] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [QRCode] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Status] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'Available' NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[ReadingRoomSeats] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of ReadingRoomSeats
-- ----------------------------
SET IDENTITY_INSERT [dbo].[ReadingRoomSeats] ON
GO

SET IDENTITY_INSERT [dbo].[ReadingRoomSeats] OFF
GO


-- ----------------------------
-- Table structure for Roles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type IN ('U'))
	DROP TABLE [dbo].[Roles]
GO

CREATE TABLE [dbo].[Roles] (
  [RoleId] int  IDENTITY(1,1) NOT NULL,
  [RoleName] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[Roles] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Roles
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Roles] ON
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'Admin', N'Quản trị viên hệ thống - Toàn quyền quản lý', N'2025-11-19 10:19:31.053', N'2025-11-19 10:19:31.053')
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'Librarian', N'Thủ thư - Quản lý mượn trả, kho sách', N'2025-11-19 10:19:31.053', N'2025-11-19 10:19:31.053')
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'3', N'Reader', N'Độc giả - Mượn sách, tìm kiếm', N'2025-11-19 10:19:31.053', N'2025-11-19 10:19:31.053')
GO

SET IDENTITY_INSERT [dbo].[Roles] OFF
GO


-- ----------------------------
-- Table structure for SystemSettings
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND type IN ('U'))
	DROP TABLE [dbo].[SystemSettings]
GO

CREATE TABLE [dbo].[SystemSettings] (
  [SettingId] int  IDENTITY(1,1) NOT NULL,
  [SettingKey] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [SettingValue] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Category] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [UpdatedBy] int  NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL
)
GO

ALTER TABLE [dbo].[SystemSettings] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of SystemSettings
-- ----------------------------
SET IDENTITY_INSERT [dbo].[SystemSettings] ON
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'1', N'MaxBorrowDays', N'14', N'Số ngày mượn tối đa', N'Borrowing', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'2', N'MaxRenewDays', N'7', N'Số ngày gia hạn tối đa', N'Borrowing', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'3', N'MaxBorrowBooks', N'5', N'Số sách mượn tối đa cùng lúc', N'Borrowing', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'4', N'FinePerDay', N'5000', N'Phí phạt mỗi ngày quá hạn (VNĐ)', N'Fine', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'5', N'ReservationExpiryDays', N'3', N'Số ngày hết hạn đặt sách', N'Reservation', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'6', N'EmailReturnReminderDays', N'2', N'Số ngày trước khi trả để gửi email nhắc', N'Notification', NULL, N'2025-11-19 10:19:31.060')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'7', N'EmailOverdueAlertDays', N'1', N'Số ngày quá hạn để gửi email cảnh báo', N'Notification', NULL, N'2025-11-19 10:19:31.060')
GO

SET IDENTITY_INSERT [dbo].[SystemSettings] OFF
GO


-- ----------------------------
-- Table structure for UserRoles
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type IN ('U'))
	DROP TABLE [dbo].[UserRoles]
GO

CREATE TABLE [dbo].[UserRoles] (
  [UserRoleId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [RoleId] int  NOT NULL,
  [AssignedAt] datetime DEFAULT getdate() NULL,
  [AssignedBy] int  NULL
)
GO

ALTER TABLE [dbo].[UserRoles] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of UserRoles
-- ----------------------------
SET IDENTITY_INSERT [dbo].[UserRoles] ON
GO

SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO


-- ----------------------------
-- Table structure for Users
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type IN ('U'))
	DROP TABLE [dbo].[Users]
GO

CREATE TABLE [dbo].[Users] (
  [UserId] int  IDENTITY(1,1) NOT NULL,
  [Username] nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [Email] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [PasswordHash] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [FullName] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [PhoneNumber] nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [Address] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [DateOfBirth] date  NULL,
  [Gender] nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [IsActive] bit DEFAULT 1 NULL,
  [IsLocked] bit DEFAULT 0 NULL,
  [LockedUntil] datetime  NULL,
  [FailedLoginAttempts] int DEFAULT 0 NULL,
  [CreatedAt] datetime DEFAULT getdate() NULL,
  [UpdatedAt] datetime DEFAULT getdate() NULL,
  [LastLoginAt] datetime  NULL
)
GO

ALTER TABLE [dbo].[Users] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of Users
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Users] ON
GO

SET IDENTITY_INSERT [dbo].[Users] OFF
GO


-- ----------------------------
-- View structure for VW_BorrowStatisticsByMonth
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[VW_BorrowStatisticsByMonth]') AND type IN ('V'))
	DROP VIEW [dbo].[VW_BorrowStatisticsByMonth]
GO

CREATE VIEW [dbo].[VW_BorrowStatisticsByMonth] AS SELECT 
    YEAR(br.BorrowDate) AS Year,
    MONTH(br.BorrowDate) AS Month,
    COUNT(DISTINCT br.BorrowId) AS TotalBorrows,
    COUNT(DISTINCT br.UserId) AS TotalBorrowers,
    COUNT(DISTINCT bc.BookId) AS TotalBooks,
    COUNT(DISTINCT CASE WHEN br.ReturnDate IS NOT NULL THEN br.BorrowId END) AS TotalReturns,
    COUNT(DISTINCT CASE WHEN br.DueDate < GETDATE() AND br.ReturnDate IS NULL THEN br.BorrowId END) AS OverdueBorrows,
    SUM(br.FineAmount) AS TotalFines,
    SUM(br.FinePaid) AS TotalFinesPaid
FROM Borrows br
LEFT JOIN BookCopies bc ON br.CopyId = bc.CopyId
GROUP BY YEAR(br.BorrowDate), MONTH(br.BorrowDate);
GO


-- ----------------------------
-- View structure for VW_BorrowStatisticsByYear
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[VW_BorrowStatisticsByYear]') AND type IN ('V'))
	DROP VIEW [dbo].[VW_BorrowStatisticsByYear]
GO

CREATE VIEW [dbo].[VW_BorrowStatisticsByYear] AS SELECT 
    YEAR(br.BorrowDate) AS Year,
    COUNT(DISTINCT br.BorrowId) AS TotalBorrows,
    COUNT(DISTINCT br.UserId) AS TotalBorrowers,
    COUNT(DISTINCT bc.BookId) AS TotalBooks,
    COUNT(DISTINCT CASE WHEN br.ReturnDate IS NOT NULL THEN br.BorrowId END) AS TotalReturns,
    SUM(br.FineAmount) AS TotalFines,
    SUM(br.FinePaid) AS TotalFinesPaid
FROM Borrows br
LEFT JOIN BookCopies bc ON br.CopyId = bc.CopyId
GROUP BY YEAR(br.BorrowDate);
GO


-- ----------------------------
-- View structure for VW_MostActiveReaders
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[VW_MostActiveReaders]') AND type IN ('V'))
	DROP VIEW [dbo].[VW_MostActiveReaders]
GO

CREATE VIEW [dbo].[VW_MostActiveReaders] AS SELECT 
    u.UserId,
    u.FullName,
    u.Email,
    lc.CardNumber,
    COUNT(DISTINCT br.BorrowId) AS TotalBorrows,
    COUNT(DISTINCT CASE WHEN br.Status = 'Borrowed' THEN br.BorrowId END) AS CurrentBorrows,
    COUNT(DISTINCT CASE WHEN br.Status = 'Overdue' THEN br.BorrowId END) AS OverdueBorrows,
    SUM(br.FineAmount) AS TotalFines,
    SUM(br.FinePaid) AS TotalFinesPaid
FROM Users u
LEFT JOIN LibraryCards lc ON u.UserId = lc.UserId
LEFT JOIN Borrows br ON u.UserId = br.UserId
LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
LEFT JOIN Roles r ON ur.RoleId = r.RoleId
WHERE r.RoleName = 'Reader'
GROUP BY u.UserId, u.FullName, u.Email, lc.CardNumber;
GO


-- ----------------------------
-- View structure for VW_MostBorrowedBooks
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[VW_MostBorrowedBooks]') AND type IN ('V'))
	DROP VIEW [dbo].[VW_MostBorrowedBooks]
GO

CREATE VIEW [dbo].[VW_MostBorrowedBooks] AS SELECT 
    b.BookId,
    b.Title,
    b.ISBN,
    c.CategoryName,
    COUNT(br.BorrowId) AS TotalBorrows,
    COUNT(DISTINCT br.UserId) AS TotalBorrowers,
    b.TotalCopies,
    b.AvailableCopies
FROM Books b
LEFT JOIN BookCopies bc ON b.BookId = bc.BookId
LEFT JOIN Borrows br ON bc.CopyId = br.CopyId
LEFT JOIN Categories c ON b.CategoryId = c.CategoryId
GROUP BY b.BookId, b.Title, b.ISBN, c.CategoryName, b.TotalCopies, b.AvailableCopies;
GO


-- ----------------------------
-- View structure for VW_OverdueBooks
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[VW_OverdueBooks]') AND type IN ('V'))
	DROP VIEW [dbo].[VW_OverdueBooks]
GO

CREATE VIEW [dbo].[VW_OverdueBooks] AS SELECT 
    br.BorrowId,
    br.BorrowNumber,
    u.UserId,
    u.FullName,
    u.Email,
    u.PhoneNumber,
    b.BookId,
    b.Title,
    bc.CopyNumber,
    br.BorrowDate,
    br.DueDate,
    DATEDIFF(DAY, br.DueDate, GETDATE()) AS DaysOverdue,
    br.FineAmount,
    br.Status
FROM Borrows br
INNER JOIN Users u ON br.UserId = u.UserId
INNER JOIN BookCopies bc ON br.CopyId = bc.CopyId
INNER JOIN Books b ON bc.BookId = b.BookId
WHERE br.Status = 'Borrowed' 
AND br.DueDate < GETDATE()
AND br.ReturnDate IS NULL;
GO


-- ----------------------------
-- Auto increment value for Authors
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Authors]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table Authors
-- ----------------------------
ALTER TABLE [dbo].[Authors] ADD CONSTRAINT [PK__Authors__70DAFC34499571A9] PRIMARY KEY CLUSTERED ([AuthorId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookAuthors
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookAuthors]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [UQ__BookAuth__6AED6DC504D01821] UNIQUE NONCLUSTERED ([BookId] ASC, [AuthorId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [PK__BookAuth__21B24F59290EF61E] PRIMARY KEY CLUSTERED ([BookAuthorId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookCopies
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookCopies]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table BookCopies
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_BookCopies_BookId]
ON [dbo].[BookCopies] (
  [BookId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_BookCopies_Status]
ON [dbo].[BookCopies] (
  [Status] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_BookCopies_Barcode]
ON [dbo].[BookCopies] (
  [Barcode] ASC
)
GO


-- ----------------------------
-- Triggers structure for table BookCopies
-- ----------------------------
CREATE TRIGGER [dbo].[TRG_BookCopies_UpdateAvailableCopies_Insert]
ON [dbo].[BookCopies]
WITH EXECUTE AS CALLER
FOR INSERT
AS
BEGIN
    UPDATE Books
    SET AvailableCopies = (
        SELECT COUNT(*) 
        FROM BookCopies 
        WHERE BookId = inserted.BookId 
        AND Status = 'Available'
    ),
    TotalCopies = (
        SELECT COUNT(*) 
        FROM BookCopies 
        WHERE BookId = inserted.BookId
    )
    FROM inserted
    WHERE Books.BookId = inserted.BookId;
END;
GO

CREATE TRIGGER [dbo].[TRG_BookCopies_UpdateAvailableCopies_Update]
ON [dbo].[BookCopies]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
    IF UPDATE(Status)
    BEGIN
        UPDATE Books
        SET AvailableCopies = (
            SELECT COUNT(*) 
            FROM BookCopies 
            WHERE BookId = inserted.BookId 
            AND Status = 'Available'
        ),
        BorrowedCopies = (
            SELECT COUNT(*) 
            FROM BookCopies 
            WHERE BookId = inserted.BookId 
            AND Status = 'Borrowed'
        ),
        LostCopies = (
            SELECT COUNT(*) 
            FROM BookCopies 
            WHERE BookId = inserted.BookId 
            AND Status = 'Lost'
        ),
        DamagedCopies = (
            SELECT COUNT(*) 
            FROM BookCopies 
            WHERE BookId = inserted.BookId 
            AND Status = 'Damaged'
        )
        FROM inserted
        WHERE Books.BookId = inserted.BookId;
    END
END;
GO


-- ----------------------------
-- Uniques structure for table BookCopies
-- ----------------------------
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [UQ__BookCopi__177800D39CC96AB7] UNIQUE NONCLUSTERED ([Barcode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [UQ__BookCopi__C02668298289461A] UNIQUE NONCLUSTERED ([BookId] ASC, [CopyNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookCopies
-- ----------------------------
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [PK__BookCopi__C26CCCC53CC32940] PRIMARY KEY CLUSTERED ([CopyId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookDamages
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookDamages]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table BookDamages
-- ----------------------------
ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [PK__BookDama__8A0F2162B3A92208] PRIMARY KEY CLUSTERED ([DamageId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookImports
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookImports]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table BookImports
-- ----------------------------
ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [PK__BookImpo__869767EA55165E87] PRIMARY KEY CLUSTERED ([ImportId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookLocations
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookLocations]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table BookLocations
-- ----------------------------
ALTER TABLE [dbo].[BookLocations] ADD CONSTRAINT [UQ__BookLoca__DDB144D560BD6F5D] UNIQUE NONCLUSTERED ([LocationCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookLocations
-- ----------------------------
ALTER TABLE [dbo].[BookLocations] ADD CONSTRAINT [PK__BookLoca__E7FEA497B7A40108] PRIMARY KEY CLUSTERED ([LocationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookReservations
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookReservations]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table BookReservations
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_BookReservations_UserId]
ON [dbo].[BookReservations] (
  [UserId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_BookReservations_BookId]
ON [dbo].[BookReservations] (
  [BookId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_BookReservations_Status]
ON [dbo].[BookReservations] (
  [Status] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table BookReservations
-- ----------------------------
ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [PK__BookRese__B7EE5F24E0650B23] PRIMARY KEY CLUSTERED ([ReservationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookReviews
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookReviews]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [UQ__BookRevi__EC984EC224A7C5B0] UNIQUE NONCLUSTERED ([BookId] ASC, [UserId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Checks structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [CK__BookRevie__Ratin__47A6A41B] CHECK ([Rating]>=(1) AND [Rating]<=(5))
GO


-- ----------------------------
-- Primary Key structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [PK__BookRevi__74BC79CEC82B28C4] PRIMARY KEY CLUSTERED ([ReviewId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Books
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Books]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table Books
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Books_Title]
ON [dbo].[Books] (
  [Title] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Books_ISBN]
ON [dbo].[Books] (
  [ISBN] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Books_CategoryId]
ON [dbo].[Books] (
  [CategoryId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Books_Status]
ON [dbo].[Books] (
  [Status] ASC
)
GO


-- ----------------------------
-- Uniques structure for table Books
-- ----------------------------
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [UQ__Books__447D36EA4A0559A1] UNIQUE NONCLUSTERED ([ISBN] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Books
-- ----------------------------
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [PK__Books__3DE0C207B9616D41] PRIMARY KEY CLUSTERED ([BookId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BorrowHistory
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BorrowHistory]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table BorrowHistory
-- ----------------------------
ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [PK__BorrowHi__4D7B4ABDF407BE7A] PRIMARY KEY CLUSTERED ([HistoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Borrows
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Borrows]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table Borrows
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Borrows_UserId]
ON [dbo].[Borrows] (
  [UserId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Borrows_CopyId]
ON [dbo].[Borrows] (
  [CopyId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Borrows_Status]
ON [dbo].[Borrows] (
  [Status] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Borrows_DueDate]
ON [dbo].[Borrows] (
  [DueDate] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Borrows_BorrowDate]
ON [dbo].[Borrows] (
  [BorrowDate] ASC
)
GO


-- ----------------------------
-- Triggers structure for table Borrows
-- ----------------------------
CREATE TRIGGER [dbo].[TRG_Borrows_CalculateFine]
ON [dbo].[Borrows]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
    IF UPDATE(ReturnDate) AND EXISTS (SELECT 1 FROM inserted WHERE ReturnDate IS NOT NULL)
    BEGIN
        UPDATE Borrows
        SET FineAmount = CASE 
            WHEN DATEDIFF(DAY, inserted.DueDate, inserted.ReturnDate) > 0 
            THEN DATEDIFF(DAY, inserted.DueDate, inserted.ReturnDate) * 
                 CAST((SELECT SettingValue FROM SystemSettings WHERE SettingKey = 'FinePerDay') AS DECIMAL(18,2))
            ELSE 0
        END
        FROM inserted
        WHERE Borrows.BorrowId = inserted.BorrowId
        AND inserted.ReturnDate IS NOT NULL
        AND inserted.ReturnDate > inserted.DueDate;
    END
END;
GO


-- ----------------------------
-- Uniques structure for table Borrows
-- ----------------------------
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [UQ__Borrows__2E556623DC7C04E3] UNIQUE NONCLUSTERED ([BorrowNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Borrows
-- ----------------------------
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [PK__Borrows__4295F83FEB05E4C1] PRIMARY KEY CLUSTERED ([BorrowId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Categories
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Categories]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table Categories
-- ----------------------------
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [UQ__Categori__8517B2E02A269F3A] UNIQUE NONCLUSTERED ([CategoryName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Categories
-- ----------------------------
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [PK__Categori__19093A0B1E9BAAD1] PRIMARY KEY CLUSTERED ([CategoryId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for EmailLogs
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[EmailLogs]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table EmailLogs
-- ----------------------------
ALTER TABLE [dbo].[EmailLogs] ADD CONSTRAINT [PK__EmailLog__E8CB41CCF8A7EC55] PRIMARY KEY CLUSTERED ([EmailLogId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for InventoryCheckDetails
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[InventoryCheckDetails]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table InventoryCheckDetails
-- ----------------------------
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [UQ__Inventor__CAA79BAB9B757D1C] UNIQUE NONCLUSTERED ([CheckId] ASC, [CopyId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table InventoryCheckDetails
-- ----------------------------
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [PK__Inventor__135C316D4F99C9BF] PRIMARY KEY CLUSTERED ([DetailId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for InventoryChecks
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[InventoryChecks]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table InventoryChecks
-- ----------------------------
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [UQ__Inventor__C6E286383E051F2F] UNIQUE NONCLUSTERED ([CheckNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table InventoryChecks
-- ----------------------------
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [PK__Inventor__8681576640365B02] PRIMARY KEY CLUSTERED ([CheckId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for LibraryCards
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[LibraryCards]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table LibraryCards
-- ----------------------------
ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [UQ__LibraryC__1788CC4DAD3E7E0D] UNIQUE NONCLUSTERED ([UserId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [UQ__LibraryC__A4E9FFE9FFC575B5] UNIQUE NONCLUSTERED ([CardNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table LibraryCards
-- ----------------------------
ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [PK__LibraryC__55FECDAE504F0564] PRIMARY KEY CLUSTERED ([CardId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Notifications
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Notifications]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table Notifications
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId]
ON [dbo].[Notifications] (
  [UserId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Notifications_IsRead]
ON [dbo].[Notifications] (
  [IsRead] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Notifications_CreatedAt]
ON [dbo].[Notifications] (
  [CreatedAt] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table Notifications
-- ----------------------------
ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [PK__Notifica__20CF2E127C960A97] PRIMARY KEY CLUSTERED ([NotificationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for PasswordResetTokens
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[PasswordResetTokens]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table PasswordResetTokens
-- ----------------------------
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [UQ__Password__1EB4F8174060A0E4] UNIQUE NONCLUSTERED ([Token] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table PasswordResetTokens
-- ----------------------------
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [PK__Password__658FEEEA1ACB44DB] PRIMARY KEY CLUSTERED ([TokenId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Publishers
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Publishers]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table Publishers
-- ----------------------------
ALTER TABLE [dbo].[Publishers] ADD CONSTRAINT [UQ__Publishe__5F0E2249A5D6DCD6] UNIQUE NONCLUSTERED ([PublisherName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Publishers
-- ----------------------------
ALTER TABLE [dbo].[Publishers] ADD CONSTRAINT [PK__Publishe__4C657FABDBD5AF18] PRIMARY KEY CLUSTERED ([PublisherId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ReadingRoomReservations
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ReadingRoomReservations]', RESEED, 1)
GO


-- ----------------------------
-- Primary Key structure for table ReadingRoomReservations
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [PK__ReadingR__B7EE5F240D0E8289] PRIMARY KEY CLUSTERED ([ReservationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ReadingRooms
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ReadingRooms]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table ReadingRooms
-- ----------------------------
ALTER TABLE [dbo].[ReadingRooms] ADD CONSTRAINT [UQ__ReadingR__4F9D52314B7DA6C5] UNIQUE NONCLUSTERED ([RoomCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ReadingRooms
-- ----------------------------
ALTER TABLE [dbo].[ReadingRooms] ADD CONSTRAINT [PK__ReadingR__328639391EDB4FB2] PRIMARY KEY CLUSTERED ([RoomId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for ReadingRoomSeats
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[ReadingRoomSeats]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table ReadingRoomSeats
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [UQ__ReadingR__2C64E5880E1F2905] UNIQUE NONCLUSTERED ([RoomId] ASC, [SeatNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [UQ__ReadingR__5B869AD9A0299639] UNIQUE NONCLUSTERED ([QRCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ReadingRoomSeats
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [PK__ReadingR__311713F3344279BC] PRIMARY KEY CLUSTERED ([SeatId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Roles
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Roles]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table Roles
-- ----------------------------
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [UQ__Roles__8A2B616021C21959] UNIQUE NONCLUSTERED ([RoleName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Roles
-- ----------------------------
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK__Roles__8AFACE1A51A3C513] PRIMARY KEY CLUSTERED ([RoleId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for SystemSettings
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[SystemSettings]', RESEED, 7)
GO


-- ----------------------------
-- Uniques structure for table SystemSettings
-- ----------------------------
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [UQ__SystemSe__01E719AD2377A5D1] UNIQUE NONCLUSTERED ([SettingKey] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table SystemSettings
-- ----------------------------
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [PK__SystemSe__54372B1D2E9892DC] PRIMARY KEY CLUSTERED ([SettingId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for UserRoles
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[UserRoles]', RESEED, 1)
GO


-- ----------------------------
-- Uniques structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [UQ__UserRole__AF2760ACB89C9468] UNIQUE NONCLUSTERED ([UserId] ASC, [RoleId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [PK__UserRole__3D978A35015EFBBC] PRIMARY KEY CLUSTERED ([UserRoleId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Users
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 1)
GO


-- ----------------------------
-- Indexes structure for table Users
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Users_Email]
ON [dbo].[Users] (
  [Email] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Users_Username]
ON [dbo].[Users] (
  [Username] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Users_IsActive]
ON [dbo].[Users] (
  [IsActive] ASC
)
GO


-- ----------------------------
-- Triggers structure for table Users
-- ----------------------------
CREATE TRIGGER [dbo].[TRG_Users_UpdateUpdatedAt]
ON [dbo].[Users]
WITH EXECUTE AS CALLER
FOR UPDATE
AS
BEGIN
    UPDATE Users
    SET UpdatedAt = GETDATE()
    FROM inserted
    WHERE Users.UserId = inserted.UserId;
END;
GO


-- ----------------------------
-- Uniques structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ__Users__536C85E42758209C] UNIQUE NONCLUSTERED ([Username] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ__Users__A9D10534077ACBF3] UNIQUE NONCLUSTERED ([Email] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK__Users__1788CC4C02E2353E] PRIMARY KEY CLUSTERED ([UserId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [FK__BookAutho__BookI__7E37BEF6] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [FK__BookAutho__Autho__7F2BE32F] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Authors] ([AuthorId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookCopies
-- ----------------------------
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [FK__BookCopie__BookI__07C12930] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookDamages
-- ----------------------------
ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__CopyI__2FCF1A8A] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__Repor__30C33EC3] FOREIGN KEY ([ReportedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__Proce__31B762FC] FOREIGN KEY ([ProcessedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookImports
-- ----------------------------
ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [FK__BookImpor__BookI__0C85DE4D] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [FK__BookImpor__Impor__0D7A0286] FOREIGN KEY ([ImportedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookReservations
-- ----------------------------
ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__UserI__14270015] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__BookI__151B244E] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__Appro__160F4887] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__BookI__4B7734FF] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__UserI__4C6B5938] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__Appro__4D5F7D71] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Books
-- ----------------------------
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__CategoryI__76969D2E] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__Publisher__778AC167] FOREIGN KEY ([PublisherId]) REFERENCES [dbo].[Publishers] ([PublisherId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__LocationI__787EE5A0] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[BookLocations] ([LocationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__CreatedBy__797309D9] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BorrowHistory
-- ----------------------------
ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__Borro__2739D489] FOREIGN KEY ([BorrowId]) REFERENCES [dbo].[Borrows] ([BorrowId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__UserI__282DF8C2] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__CopyI__29221CFB] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Borrows
-- ----------------------------
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__UserId__1F98B2C1] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__CopyId__208CD6FA] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Reserva__2180FB33] FOREIGN KEY ([ReservationId]) REFERENCES [dbo].[BookReservations] ([ReservationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Borrowe__22751F6C] FOREIGN KEY ([BorrowedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Returne__236943A5] FOREIGN KEY ([ReturnedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table EmailLogs
-- ----------------------------
ALTER TABLE [dbo].[EmailLogs] ADD CONSTRAINT [FK__EmailLogs__UserI__6DCC4D03] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table InventoryCheckDetails
-- ----------------------------
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [FK__Inventory__Check__3E1D39E1] FOREIGN KEY ([CheckId]) REFERENCES [dbo].[InventoryChecks] ([CheckId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [FK__Inventory__CopyI__3F115E1A] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table InventoryChecks
-- ----------------------------
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [FK__Inventory__Check__395884C4] FOREIGN KEY ([CheckedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table LibraryCards
-- ----------------------------
ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [FK__LibraryCa__UserI__5165187F] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [FK__LibraryCa__Creat__52593CB8] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Notifications
-- ----------------------------
ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__UserI__671F4F74] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__Relat__681373AD] FOREIGN KEY ([RelatedBorrowId]) REFERENCES [dbo].[Borrows] ([BorrowId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__Relat__690797E6] FOREIGN KEY ([RelatedReservationId]) REFERENCES [dbo].[BookReservations] ([ReservationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table PasswordResetTokens
-- ----------------------------
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [FK__PasswordR__UserI__5812160E] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ReadingRoomReservations
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [FK__ReadingRo__UserI__607251E5] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [FK__ReadingRo__SeatI__6166761E] FOREIGN KEY ([SeatId]) REFERENCES [dbo].[ReadingRoomSeats] ([SeatId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ReadingRoomSeats
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [FK__ReadingRo__RoomI__5AB9788F] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[ReadingRooms] ([RoomId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table SystemSettings
-- ----------------------------
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [FK__SystemSet__Updat__43D61337] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [FK__UserRoles__UserI__46E78A0C] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [FK__UserRoles__RoleI__47DBAE45] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [FK__UserRoles__Assig__48CFD27E] FOREIGN KEY ([AssignedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

