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

 Date: 19/11/2025 23:04:11
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

INSERT INTO [dbo].[Authors] ([AuthorId], [AuthorName], [Biography], [Nationality], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'Nguyễn Nhật Ánh', NULL, N'Việt Nam', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
GO

INSERT INTO [dbo].[Authors] ([AuthorId], [AuthorName], [Biography], [Nationality], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'Robert C. Martin', NULL, N'Mỹ', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
GO

INSERT INTO [dbo].[Authors] ([AuthorId], [AuthorName], [Biography], [Nationality], [CreatedAt], [UpdatedAt]) VALUES (N'3', N'Dũng', N'1 tuổi lập trình
2 tuổi viết sách', N'Việt Nam', N'2025-11-19 22:32:23.023', N'2025-11-19 22:32:23.023')
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

INSERT INTO [dbo].[BookAuthors] ([BookAuthorId], [BookId], [AuthorId], [IsPrimary]) VALUES (N'1', N'1', N'1', N'0')
GO

INSERT INTO [dbo].[BookAuthors] ([BookAuthorId], [BookId], [AuthorId], [IsPrimary]) VALUES (N'2', N'2', N'2', N'0')
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

INSERT INTO [dbo].[BookCopies] ([CopyId], [BookId], [CopyNumber], [Barcode], [Status], [Condition], [PurchaseDate], [PurchasePrice], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'1', N'B001', N'TT-0001', N'Available', N'Good', NULL, NULL, NULL, N'2025-11-19 16:51:40.563', N'2025-11-19 16:51:40.563')
GO

INSERT INTO [dbo].[BookCopies] ([CopyId], [BookId], [CopyNumber], [Barcode], [Status], [Condition], [PurchaseDate], [PurchasePrice], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'1', N'B002', N'TT-0002', N'Available', N'Good', NULL, NULL, NULL, N'2025-11-19 16:51:40.563', N'2025-11-19 16:51:40.563')
GO

INSERT INTO [dbo].[BookCopies] ([CopyId], [BookId], [CopyNumber], [Barcode], [Status], [Condition], [PurchaseDate], [PurchasePrice], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'3', N'1', N'B003', N'TT-0003', N'Available', N'Good', NULL, NULL, NULL, N'2025-11-19 16:51:40.563', N'2025-11-19 16:51:40.563')
GO

INSERT INTO [dbo].[BookCopies] ([CopyId], [BookId], [CopyNumber], [Barcode], [Status], [Condition], [PurchaseDate], [PurchasePrice], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'4', N'2', N'B001', N'CC-0001', N'Available', N'Good', NULL, NULL, NULL, N'2025-11-19 16:51:40.563', N'2025-11-19 16:51:40.563')
GO

INSERT INTO [dbo].[BookCopies] ([CopyId], [BookId], [CopyNumber], [Barcode], [Status], [Condition], [PurchaseDate], [PurchasePrice], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'5', N'2', N'B002', N'CC-0002', N'Available', N'Good', NULL, NULL, NULL, N'2025-11-19 16:51:40.563', N'2025-11-19 16:51:40.563')
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

INSERT INTO [dbo].[BookLocations] ([LocationId], [LocationCode], [ShelfNumber], [RowNumber], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'A-01-001', N'A-01', N'01', N'Kệ văn học thiếu nhi', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
GO

INSERT INTO [dbo].[BookLocations] ([LocationId], [LocationCode], [ShelfNumber], [RowNumber], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'B-02-005', N'B-02', N'05', N'Kệ CNTT', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
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

INSERT INTO [dbo].[BookReservations] ([ReservationId], [UserId], [BookId], [ReservationDate], [Status], [ExpiryDate], [ApprovedBy], [ApprovedAt], [RejectionReason], [Notes], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'3', N'2', N'2025-11-19 16:51:40.593', N'Pending', N'2025-11-22 16:51:40.593', NULL, NULL, NULL, NULL, N'2025-11-19 16:51:40.593', N'2025-11-19 16:51:40.593')
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

INSERT INTO [dbo].[Books] ([BookId], [ISBN], [Title], [Description], [CoverImage], [Language], [PublicationYear], [PageCount], [CategoryId], [PublisherId], [LocationId], [TotalCopies], [AvailableCopies], [BorrowedCopies], [LostCopies], [DamagedCopies], [Status], [CreatedBy], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'9786042114034', N'Tôi thấy hoa vàng trên cỏ xanh', N'Truyện dài nổi tiếng của Nguyễn Nhật Ánh', NULL, N'Tiếng Việt', N'2015', N'380', N'1', N'1', N'1', N'3', N'3', N'0', N'0', N'0', N'Active', N'1', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
GO

INSERT INTO [dbo].[Books] ([BookId], [ISBN], [Title], [Description], [CoverImage], [Language], [PublicationYear], [PageCount], [CategoryId], [PublisherId], [LocationId], [TotalCopies], [AvailableCopies], [BorrowedCopies], [LostCopies], [DamagedCopies], [Status], [CreatedBy], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'9780132350884', N'Clean Code', N'Handbook of Agile Software Craftsmanship', NULL, N'English', N'2008', N'464', N'2', N'2', N'2', N'2', N'2', N'0', N'0', N'0', N'Active', N'1', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
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

INSERT INTO [dbo].[Borrows] ([BorrowId], [BorrowNumber], [UserId], [CopyId], [ReservationId], [BorrowDate], [DueDate], [ReturnDate], [Status], [FineAmount], [FinePaid], [ConditionOnBorrow], [ConditionOnReturn], [Notes], [BorrowedBy], [ReturnedBy], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'BR240001', N'3', N'1', NULL, N'2025-11-19', N'2025-12-03', NULL, N'Borrowed', N'0.00', N'0.00', NULL, NULL, NULL, N'2', NULL, N'2025-11-19 16:51:40.597', N'2025-11-19 16:51:40.597')
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

INSERT INTO [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'Văn học', N'Sách văn học Việt Nam và thế giới', N'2025-11-19 16:51:40.547', N'2025-11-19 16:51:40.547')
GO

INSERT INTO [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'Công nghệ thông tin', N'Sách CNTT, lập trình', N'2025-11-19 16:51:40.547', N'2025-11-19 16:51:40.547')
GO

INSERT INTO [dbo].[Categories] ([CategoryId], [CategoryName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'3', N'Toán Học', N'Học Toán Nè', N'2025-11-19 22:31:27.493', N'2025-11-19 22:31:38.870')
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

INSERT INTO [dbo].[LibraryCards] ([CardId], [UserId], [CardNumber], [IssueDate], [ExpiryDate], [Status], [CreatedBy], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'3', N'LC-00001', N'2025-11-19', N'2027-11-19', N'Active', N'2', N'2025-11-19 16:51:40.593', N'2025-11-19 16:51:40.593')
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

INSERT INTO [dbo].[Notifications] ([NotificationId], [UserId], [NotificationType], [Title], [Message], [IsRead], [IsEmailSent], [EmailSentAt], [RelatedBorrowId], [RelatedReservationId], [CreatedAt], [ReadAt]) VALUES (N'1', N'3', N'System', N'Chào mừng đến thư viện', N'Bạn đã đăng ký thành công tài khoản thư viện.', N'0', N'0', NULL, NULL, NULL, N'2025-11-19 16:51:40.597', NULL)
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

INSERT INTO [dbo].[Publishers] ([PublisherId], [PublisherName], [Address], [PhoneNumber], [Email], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'NXB Trẻ', N'161B Lý Chính Thắng, Q3', N'0281234567', N'contact@nxbtre.vn', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
GO

INSERT INTO [dbo].[Publishers] ([PublisherId], [PublisherName], [Address], [PhoneNumber], [Email], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'Pearson', N'221 River St, New Jersey', N'+120123456', N'info@pearson.com', N'2025-11-19 16:51:40.550', N'2025-11-19 16:51:40.550')
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
-- Table structure for RefreshTokens
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type IN ('U'))
	DROP TABLE [dbo].[RefreshTokens]
GO

CREATE TABLE [dbo].[RefreshTokens] (
  [TokenId] int  IDENTITY(1,1) NOT NULL,
  [UserId] int  NOT NULL,
  [Token] nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ExpiresAt] datetime2(7)  NOT NULL,
  [IsRevoked] bit DEFAULT 0 NOT NULL,
  [RevokedAt] datetime2(7)  NULL,
  [CreatedAt] datetime2(7) DEFAULT getdate() NOT NULL
)
GO

ALTER TABLE [dbo].[RefreshTokens] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of RefreshTokens
-- ----------------------------
SET IDENTITY_INSERT [dbo].[RefreshTokens] ON
GO

INSERT INTO [dbo].[RefreshTokens] ([TokenId], [UserId], [Token], [ExpiresAt], [IsRevoked], [RevokedAt], [CreatedAt]) VALUES (N'1', N'1', N'JMIAQLaGjAm/THlmbsBm2oXvVxlcZXppc7MMRm2/4/VXeFAbtrqtayTZ9fK3h0KP9GEOByWhFpxoRtiA2vQjIA==', N'2025-11-26 09:53:46.0603260', N'0', NULL, N'2025-11-19 09:53:46.0603987')
GO

INSERT INTO [dbo].[RefreshTokens] ([TokenId], [UserId], [Token], [ExpiresAt], [IsRevoked], [RevokedAt], [CreatedAt]) VALUES (N'2', N'1', N'Fl2PAYXHOqIcYPfEVoJmZ2fMQ9mSwFFjATk9pxsp2TaQRjlI3LLOZ91gwBNXIwRbGAw4X5WQrBMID9bdCWWltg==', N'2025-11-26 09:58:42.9707232', N'0', NULL, N'2025-11-19 09:58:42.9708905')
GO

INSERT INTO [dbo].[RefreshTokens] ([TokenId], [UserId], [Token], [ExpiresAt], [IsRevoked], [RevokedAt], [CreatedAt]) VALUES (N'3', N'1', N'N/ucf8Opmbxd47DF4+ywhsV44P5foY7x9hUTMQBa39zmlIHlV8kxmXAfJqlT33MrQDUjOqLdIqbfZHHcu0dUAg==', N'2025-11-26 15:27:07.0281276', N'0', NULL, N'2025-11-19 15:27:07.0281698')
GO

SET IDENTITY_INSERT [dbo].[RefreshTokens] OFF
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

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'1', N'Admin', N'Quản trị viên hệ thống - Toàn quyền quản lý', N'2025-11-19 16:51:11.200', N'2025-11-19 16:51:11.200')
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'2', N'Librarian', N'Thủ thư - Quản lý mượn trả, kho sách', N'2025-11-19 16:51:11.200', N'2025-11-19 16:51:11.200')
GO

INSERT INTO [dbo].[Roles] ([RoleId], [RoleName], [Description], [CreatedAt], [UpdatedAt]) VALUES (N'3', N'Reader', N'Độc giả - Mượn sách, tìm kiếm', N'2025-11-19 16:51:11.200', N'2025-11-19 16:51:11.200')
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

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'1', N'MaxBorrowDays', N'14', N'Số ngày mượn tối đa', N'Borrowing', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'2', N'MaxRenewDays', N'7', N'Số ngày gia hạn tối đa', N'Borrowing', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'3', N'MaxBorrowBooks', N'5', N'Số sách mượn tối đa cùng lúc', N'Borrowing', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'4', N'FinePerDay', N'5000', N'Phí phạt mỗi ngày quá hạn (VNĐ)', N'Fine', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'5', N'ReservationExpiryDays', N'3', N'Số ngày hết hạn đặt sách', N'Reservation', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'6', N'EmailReturnReminderDays', N'2', N'Số ngày trước khi trả để gửi email nhắc', N'Notification', NULL, N'2025-11-19 16:51:11.207')
GO

INSERT INTO [dbo].[SystemSettings] ([SettingId], [SettingKey], [SettingValue], [Description], [Category], [UpdatedBy], [UpdatedAt]) VALUES (N'7', N'EmailOverdueAlertDays', N'1', N'Số ngày quá hạn để gửi email cảnh báo', N'Notification', NULL, N'2025-11-19 16:51:11.207')
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

INSERT INTO [dbo].[UserRoles] ([UserRoleId], [UserId], [RoleId], [AssignedAt], [AssignedBy]) VALUES (N'1', N'1', N'1', N'2025-11-19 16:51:40.547', NULL)
GO

INSERT INTO [dbo].[UserRoles] ([UserRoleId], [UserId], [RoleId], [AssignedAt], [AssignedBy]) VALUES (N'2', N'2', N'2', N'2025-11-19 16:51:40.547', NULL)
GO

INSERT INTO [dbo].[UserRoles] ([UserRoleId], [UserId], [RoleId], [AssignedAt], [AssignedBy]) VALUES (N'3', N'3', N'3', N'2025-11-19 16:51:40.547', NULL)
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

INSERT INTO [dbo].[Users] ([UserId], [Username], [Email], [PasswordHash], [FullName], [PhoneNumber], [Address], [DateOfBirth], [Gender], [IsActive], [IsLocked], [LockedUntil], [FailedLoginAttempts], [CreatedAt], [UpdatedAt], [LastLoginAt]) VALUES (N'1', N'admin', N'admin@qltv.local', N'$2a$11$UFVFgsFYkPz.AuMeU4nZCORYf0d0Mspg7ASC/hs0Dhqs8mWUod7/q', N'Quản trị viên', N'0900000000', N'Văn phòng thư viện', NULL, NULL, N'0', N'0', NULL, N'0', N'2025-11-19 16:51:40.547', N'2025-11-19 22:27:07.237', N'2025-11-19 22:27:07.237')
GO

INSERT INTO [dbo].[Users] ([UserId], [Username], [Email], [PasswordHash], [FullName], [PhoneNumber], [Address], [DateOfBirth], [Gender], [IsActive], [IsLocked], [LockedUntil], [FailedLoginAttempts], [CreatedAt], [UpdatedAt], [LastLoginAt]) VALUES (N'2', N'librarian', N'librarian@qltv.local', N'$2a$11$UFVFgsFYkPz.AuMeU4nZCORYf0d0Mspg7ASC/hs0Dhqs8mWUod7/q', N'Thủ thư Trần Thư', N'0911000000', N'Phòng lưu trữ', NULL, NULL, N'0', N'0', NULL, N'0', N'2025-11-19 16:51:40.547', N'2025-11-19 16:51:40.547', NULL)
GO

INSERT INTO [dbo].[Users] ([UserId], [Username], [Email], [PasswordHash], [FullName], [PhoneNumber], [Address], [DateOfBirth], [Gender], [IsActive], [IsLocked], [LockedUntil], [FailedLoginAttempts], [CreatedAt], [UpdatedAt], [LastLoginAt]) VALUES (N'3', N'reader01', N'reader01@qltv.local', N'$2a$11$UFVFgsFYkPz.AuMeU4nZCORYf0d0Mspg7ASC/hs0Dhqs8mWUod7/q', N'Sinh viên Nguyễn An', N'0988000000', N'KTX Khu A', NULL, NULL, N'0', N'0', NULL, N'0', N'2025-11-19 16:51:40.547', N'2025-11-19 16:51:40.547', NULL)
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
DBCC CHECKIDENT ('[dbo].[Authors]', RESEED, 3)
GO


-- ----------------------------
-- Primary Key structure for table Authors
-- ----------------------------
ALTER TABLE [dbo].[Authors] ADD CONSTRAINT [PK__Authors__70DAFC34BD12385B] PRIMARY KEY CLUSTERED ([AuthorId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookAuthors
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookAuthors]', RESEED, 2)
GO


-- ----------------------------
-- Uniques structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [UQ__BookAuth__6AED6DC56C227DF5] UNIQUE NONCLUSTERED ([BookId] ASC, [AuthorId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [PK__BookAuth__21B24F59889BF29D] PRIMARY KEY CLUSTERED ([BookAuthorId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookCopies
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookCopies]', RESEED, 5)
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
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [UQ__BookCopi__177800D36193E1EB] UNIQUE NONCLUSTERED ([Barcode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [UQ__BookCopi__C026682907A5D3B1] UNIQUE NONCLUSTERED ([BookId] ASC, [CopyNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookCopies
-- ----------------------------
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [PK__BookCopi__C26CCCC558F5FB57] PRIMARY KEY CLUSTERED ([CopyId])
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
ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [PK__BookDama__8A0F21624A81DD79] PRIMARY KEY CLUSTERED ([DamageId])
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
ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [PK__BookImpo__869767EA205DCC80] PRIMARY KEY CLUSTERED ([ImportId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for BookLocations
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[BookLocations]', RESEED, 2)
GO


-- ----------------------------
-- Uniques structure for table BookLocations
-- ----------------------------
ALTER TABLE [dbo].[BookLocations] ADD CONSTRAINT [UQ__BookLoca__DDB144D5922D1C27] UNIQUE NONCLUSTERED ([LocationCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table BookLocations
-- ----------------------------
ALTER TABLE [dbo].[BookLocations] ADD CONSTRAINT [PK__BookLoca__E7FEA4972492E6E0] PRIMARY KEY CLUSTERED ([LocationId])
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
ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [PK__BookRese__B7EE5F24BC1E5DEF] PRIMARY KEY CLUSTERED ([ReservationId])
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
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [UQ__BookRevi__EC984EC21E846287] UNIQUE NONCLUSTERED ([BookId] ASC, [UserId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Checks structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [CK__BookRevie__Ratin__4D5F7D71] CHECK ([Rating]>=(1) AND [Rating]<=(5))
GO


-- ----------------------------
-- Primary Key structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [PK__BookRevi__74BC79CE87C6A831] PRIMARY KEY CLUSTERED ([ReviewId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Books
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Books]', RESEED, 2)
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
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [UQ__Books__447D36EA94000EC0] UNIQUE NONCLUSTERED ([ISBN] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Books
-- ----------------------------
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [PK__Books__3DE0C2070EE5C412] PRIMARY KEY CLUSTERED ([BookId])
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
ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [PK__BorrowHi__4D7B4ABD77ABBD97] PRIMARY KEY CLUSTERED ([HistoryId])
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
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [UQ__Borrows__2E556623EB0DF56B] UNIQUE NONCLUSTERED ([BorrowNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Borrows
-- ----------------------------
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [PK__Borrows__4295F83F9CFA1014] PRIMARY KEY CLUSTERED ([BorrowId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Categories
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Categories]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table Categories
-- ----------------------------
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [UQ__Categori__8517B2E0758329C1] UNIQUE NONCLUSTERED ([CategoryName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Categories
-- ----------------------------
ALTER TABLE [dbo].[Categories] ADD CONSTRAINT [PK__Categori__19093A0B56E33CFC] PRIMARY KEY CLUSTERED ([CategoryId])
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
ALTER TABLE [dbo].[EmailLogs] ADD CONSTRAINT [PK__EmailLog__E8CB41CC3F898CE0] PRIMARY KEY CLUSTERED ([EmailLogId])
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
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [UQ__Inventor__CAA79BAB72DC4FD7] UNIQUE NONCLUSTERED ([CheckId] ASC, [CopyId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table InventoryCheckDetails
-- ----------------------------
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [PK__Inventor__135C316D0F1BEB10] PRIMARY KEY CLUSTERED ([DetailId])
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
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [UQ__Inventor__C6E286389B847B4B] UNIQUE NONCLUSTERED ([CheckNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table InventoryChecks
-- ----------------------------
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [PK__Inventor__86815766EC327904] PRIMARY KEY CLUSTERED ([CheckId])
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
ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [UQ__LibraryC__1788CC4D4F666D90] UNIQUE NONCLUSTERED ([UserId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [UQ__LibraryC__A4E9FFE9A9BF6291] UNIQUE NONCLUSTERED ([CardNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table LibraryCards
-- ----------------------------
ALTER TABLE [dbo].[LibraryCards] ADD CONSTRAINT [PK__LibraryC__55FECDAE671372C3] PRIMARY KEY CLUSTERED ([CardId])
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
ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [PK__Notifica__20CF2E12A5EB9024] PRIMARY KEY CLUSTERED ([NotificationId])
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
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [UQ__Password__1EB4F8170C84B464] UNIQUE NONCLUSTERED ([Token] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table PasswordResetTokens
-- ----------------------------
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [PK__Password__658FEEEAAA7521B2] PRIMARY KEY CLUSTERED ([TokenId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Publishers
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Publishers]', RESEED, 2)
GO


-- ----------------------------
-- Uniques structure for table Publishers
-- ----------------------------
ALTER TABLE [dbo].[Publishers] ADD CONSTRAINT [UQ__Publishe__5F0E2249F3036CF8] UNIQUE NONCLUSTERED ([PublisherName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Publishers
-- ----------------------------
ALTER TABLE [dbo].[Publishers] ADD CONSTRAINT [PK__Publishe__4C657FAB6CAF7DF5] PRIMARY KEY CLUSTERED ([PublisherId])
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
ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [PK__ReadingR__B7EE5F242D7CC4AE] PRIMARY KEY CLUSTERED ([ReservationId])
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
ALTER TABLE [dbo].[ReadingRooms] ADD CONSTRAINT [UQ__ReadingR__4F9D5231C9A7B930] UNIQUE NONCLUSTERED ([RoomCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ReadingRooms
-- ----------------------------
ALTER TABLE [dbo].[ReadingRooms] ADD CONSTRAINT [PK__ReadingR__32863939DE49259B] PRIMARY KEY CLUSTERED ([RoomId])
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
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [UQ__ReadingR__2C64E58853A36063] UNIQUE NONCLUSTERED ([RoomId] ASC, [SeatNumber] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [UQ__ReadingR__5B869AD95CC0ADAD] UNIQUE NONCLUSTERED ([QRCode] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table ReadingRoomSeats
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [PK__ReadingR__311713F37B668EDB] PRIMARY KEY CLUSTERED ([SeatId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for RefreshTokens
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[RefreshTokens]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table RefreshTokens
-- ----------------------------
ALTER TABLE [dbo].[RefreshTokens] ADD CONSTRAINT [UQ__RefreshT__1EB4F8172B4D81B4] UNIQUE NONCLUSTERED ([Token] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table RefreshTokens
-- ----------------------------
ALTER TABLE [dbo].[RefreshTokens] ADD CONSTRAINT [PK__RefreshT__658FEEEAF26539BD] PRIMARY KEY CLUSTERED ([TokenId])
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
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [UQ__Roles__8A2B61604EDAD1B4] UNIQUE NONCLUSTERED ([RoleName] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Roles
-- ----------------------------
ALTER TABLE [dbo].[Roles] ADD CONSTRAINT [PK__Roles__8AFACE1AD545CAB6] PRIMARY KEY CLUSTERED ([RoleId])
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
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [UQ__SystemSe__01E719ADDBEF372E] UNIQUE NONCLUSTERED ([SettingKey] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table SystemSettings
-- ----------------------------
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [PK__SystemSe__54372B1D80286A84] PRIMARY KEY CLUSTERED ([SettingId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for UserRoles
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[UserRoles]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [UQ__UserRole__AF2760AC2CF6F187] UNIQUE NONCLUSTERED ([UserId] ASC, [RoleId] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table UserRoles
-- ----------------------------
ALTER TABLE [dbo].[UserRoles] ADD CONSTRAINT [PK__UserRole__3D978A35220B1C3C] PRIMARY KEY CLUSTERED ([UserRoleId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for Users
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 3)
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
-- Uniques structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ__Users__536C85E487775C98] UNIQUE NONCLUSTERED ([Username] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD CONSTRAINT [UQ__Users__A9D105340C221C02] UNIQUE NONCLUSTERED ([Email] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table Users
-- ----------------------------
ALTER TABLE [dbo].[Users] ADD CONSTRAINT [PK__Users__1788CC4C932FDE5E] PRIMARY KEY CLUSTERED ([UserId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table BookAuthors
-- ----------------------------
ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [FK__BookAutho__BookI__03F0984C] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookAuthors] ADD CONSTRAINT [FK__BookAutho__Autho__04E4BC85] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Authors] ([AuthorId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookCopies
-- ----------------------------
ALTER TABLE [dbo].[BookCopies] ADD CONSTRAINT [FK__BookCopie__BookI__0D7A0286] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookDamages
-- ----------------------------
ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__CopyI__3587F3E0] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__Repor__367C1819] FOREIGN KEY ([ReportedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookDamages] ADD CONSTRAINT [FK__BookDamag__Proce__37703C52] FOREIGN KEY ([ProcessedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookImports
-- ----------------------------
ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [FK__BookImpor__BookI__123EB7A3] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookImports] ADD CONSTRAINT [FK__BookImpor__Impor__1332DBDC] FOREIGN KEY ([ImportedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookReservations
-- ----------------------------
ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__UserI__19DFD96B] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__BookI__1AD3FDA4] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReservations] ADD CONSTRAINT [FK__BookReser__Appro__1BC821DD] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BookReviews
-- ----------------------------
ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__BookI__51300E55] FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([BookId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__UserI__5224328E] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BookReviews] ADD CONSTRAINT [FK__BookRevie__Appro__531856C7] FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Books
-- ----------------------------
ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__CategoryI__7C4F7684] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([CategoryId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__Publisher__7D439ABD] FOREIGN KEY ([PublisherId]) REFERENCES [dbo].[Publishers] ([PublisherId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__LocationI__7E37BEF6] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[BookLocations] ([LocationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Books] ADD CONSTRAINT [FK__Books__CreatedBy__7F2BE32F] FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table BorrowHistory
-- ----------------------------
ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__Borro__2CF2ADDF] FOREIGN KEY ([BorrowId]) REFERENCES [dbo].[Borrows] ([BorrowId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__UserI__2DE6D218] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[BorrowHistory] ADD CONSTRAINT [FK__BorrowHis__CopyI__2EDAF651] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table Borrows
-- ----------------------------
ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__UserId__25518C17] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__CopyId__2645B050] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Reserva__2739D489] FOREIGN KEY ([ReservationId]) REFERENCES [dbo].[BookReservations] ([ReservationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Borrowe__282DF8C2] FOREIGN KEY ([BorrowedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Borrows] ADD CONSTRAINT [FK__Borrows__Returne__29221CFB] FOREIGN KEY ([ReturnedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table EmailLogs
-- ----------------------------
ALTER TABLE [dbo].[EmailLogs] ADD CONSTRAINT [FK__EmailLogs__UserI__73852659] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table InventoryCheckDetails
-- ----------------------------
ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [FK__Inventory__Check__43D61337] FOREIGN KEY ([CheckId]) REFERENCES [dbo].[InventoryChecks] ([CheckId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[InventoryCheckDetails] ADD CONSTRAINT [FK__Inventory__CopyI__44CA3770] FOREIGN KEY ([CopyId]) REFERENCES [dbo].[BookCopies] ([CopyId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table InventoryChecks
-- ----------------------------
ALTER TABLE [dbo].[InventoryChecks] ADD CONSTRAINT [FK__Inventory__Check__3F115E1A] FOREIGN KEY ([CheckedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
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
ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__UserI__6CD828CA] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__Relat__6DCC4D03] FOREIGN KEY ([RelatedBorrowId]) REFERENCES [dbo].[Borrows] ([BorrowId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Notifications] ADD CONSTRAINT [FK__Notificat__Relat__6EC0713C] FOREIGN KEY ([RelatedReservationId]) REFERENCES [dbo].[BookReservations] ([ReservationId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table PasswordResetTokens
-- ----------------------------
ALTER TABLE [dbo].[PasswordResetTokens] ADD CONSTRAINT [FK__PasswordR__UserI__5812160E] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ReadingRoomReservations
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [FK__ReadingRo__UserI__662B2B3B] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[ReadingRoomReservations] ADD CONSTRAINT [FK__ReadingRo__SeatI__671F4F74] FOREIGN KEY ([SeatId]) REFERENCES [dbo].[ReadingRoomSeats] ([SeatId]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table ReadingRoomSeats
-- ----------------------------
ALTER TABLE [dbo].[ReadingRoomSeats] ADD CONSTRAINT [FK__ReadingRo__RoomI__607251E5] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[ReadingRooms] ([RoomId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table RefreshTokens
-- ----------------------------
ALTER TABLE [dbo].[RefreshTokens] ADD CONSTRAINT [FK__RefreshTo__UserI__5DCAEF64] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table SystemSettings
-- ----------------------------
ALTER TABLE [dbo].[SystemSettings] ADD CONSTRAINT [FK__SystemSet__Updat__498EEC8D] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION
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

