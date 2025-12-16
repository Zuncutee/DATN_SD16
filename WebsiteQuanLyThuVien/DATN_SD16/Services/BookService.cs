using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using DATN_SD16.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATN_SD16.Services
{
    /// <summary>
    /// Service implementation cho Book
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRepository<BookImport> _bookImportRepository;
        private readonly IRepository<BookAuthor> _bookAuthorRepository;
        private readonly IRepository<BookCopy> _bookCopyRepository;
        private readonly LibraryDbContext _context;

        public BookService(
            IBookRepository bookRepository,
            IRepository<BookImport> bookImportRepository,
            IRepository<BookAuthor> bookAuthorRepository,
            IRepository<BookCopy> bookCopyRepository,
            LibraryDbContext context)
        {
            _bookRepository = bookRepository;
            _bookImportRepository = bookImportRepository;
            _bookAuthorRepository = bookAuthorRepository;
            _bookCopyRepository = bookCopyRepository;
            _context = context;
        }

        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(bookId);
        }

        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            return await _bookRepository.GetBookWithDetailsAsync(bookId);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string? title, string? author, int? categoryId, bool? availableOnly)
        {
            return await _bookRepository.SearchBooksAsync(title, author, categoryId, availableOnly);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _bookRepository.GetAvailableBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetMostBorrowedBooksAsync(int top = 10)
        {
            return await _bookRepository.GetMostBorrowedBooksAsync(top);
        }

        public async Task<Book> CreateBookAsync(Book book, int createdBy)
        {
            book.CreatedBy = createdBy;
            book.CreatedAt = DateTime.Now;
            book.UpdatedAt = DateTime.Now;
            book.Status = "Active";
            return await _bookRepository.AddAsync(book);
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            var existing = await _bookRepository.GetByIdAsync(book.BookId);
            if (existing == null) return false;

            existing.Title = book.Title;
            existing.ISBN = book.ISBN;
            existing.Description = book.Description;
            existing.CoverImage = string.IsNullOrWhiteSpace(book.CoverImage) ? existing.CoverImage : book.CoverImage;
            existing.Language = book.Language;
            existing.PublicationYear = book.PublicationYear;
            existing.PageCount = book.PageCount;
            existing.CategoryId = book.CategoryId;
            existing.PublisherId = book.PublisherId;
            existing.LocationId = book.LocationId;
            existing.TotalCopies = book.TotalCopies;
            existing.AvailableCopies = book.AvailableCopies;
            existing.BorrowedCopies = book.BorrowedCopies;
            existing.LostCopies = book.LostCopies;
            existing.DamagedCopies = book.DamagedCopies;
            existing.Status = book.Status;
            existing.UpdatedAt = DateTime.Now;

            await _bookRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null) return false;

            book.Status = "Archived";
            await _bookRepository.UpdateAsync(book);
            return true;
        }

        public async Task<bool> ImportBooksAsync(int bookId, int quantity, int importedBy)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null) return false;

            // Clean up dữ liệu cũ: Update các record có CopyNumber NULL hoặc empty
            var allCopiesForBook = await _context.BookCopies
                .Where(bc => bc.BookId == bookId)
                .ToListAsync();
            
            var nullCopies = allCopiesForBook
                .Where(bc => string.IsNullOrWhiteSpace(bc.CopyNumber))
                .OrderBy(bc => bc.CopyId)
                .ToList();

            if (nullCopies.Any())
            {
                // Lấy max sequence number từ các CopyNumber hợp lệ
                // Format: B{BookId}-{sequence}
                var allValidCopiesForCleanup = allCopiesForBook
                    .Where(bc => !string.IsNullOrWhiteSpace(bc.CopyNumber))
                    .Select(bc => bc.CopyNumber)
                    .ToList();

                var cleanupPrefix = $"B{bookId}-";
                int cleanupMaxSequence = 0;
                
                if (allValidCopiesForCleanup.Any())
                {
                    var cleanupSequences = allValidCopiesForCleanup
                        .Select(cn =>
                        {
                            var trimmed = cn.Trim();
                            // Kiểm tra format B{BookId}-{sequence}
                            if (trimmed.StartsWith(cleanupPrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                var sequenceStr = trimmed.Substring(cleanupPrefix.Length);
                                if (int.TryParse(sequenceStr, out var seq))
                                {
                                    return seq;
                                }
                            }
                            // Kiểm tra format cũ B{sequence} (backward compatibility)
                            else if (trimmed.StartsWith("B", StringComparison.OrdinalIgnoreCase))
                            {
                                var numberStr = trimmed.Substring(1).Replace("-", "");
                                if (int.TryParse(numberStr, out var num))
                                {
                                    return num;
                                }
                            }
                            return 0;
                        })
                        .Where(seq => seq > 0)
                        .ToList();

                    if (cleanupSequences.Any())
                    {
                        cleanupMaxSequence = cleanupSequences.Max();
                    }
                }

                // Lấy max Barcode sequence để tạo Barcode unique khi clean up
                var cleanupBarcodePrefix = $"BC-{bookId}-";
                var cleanupMaxBarcodeSequence = allCopiesForBook
                    .Where(bc => !string.IsNullOrWhiteSpace(bc.Barcode))
                    .Select(bc =>
                    {
                        var trimmed = bc.Barcode!.Trim();
                        if (trimmed.StartsWith(cleanupBarcodePrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            var sequenceStr = trimmed.Substring(cleanupBarcodePrefix.Length);
                            if (int.TryParse(sequenceStr, out var seq))
                            {
                                return seq;
                            }
                        }
                        return 0;
                    })
                    .Where(seq => seq > 0)
                    .DefaultIfEmpty(0)
                    .Max();

                // Update từng record NULL với CopyNumber và Barcode hợp lệ bằng SQL trực tiếp
                // Chỉ update các record có CopyId > 0 (đã tồn tại trong database)
                foreach (var nullCopy in nullCopies.Where(bc => bc.CopyId > 0))
                {
                    cleanupMaxSequence++;
                    var newCopyNumber = $"{cleanupPrefix}{cleanupMaxSequence:D3}";
                    
                    cleanupMaxBarcodeSequence++;
                    var newBarcode = $"{cleanupBarcodePrefix}{cleanupMaxBarcodeSequence:D3}";
                    
                    // Update trực tiếp bằng SQL để đảm bảo update đúng record
                    var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                        @"UPDATE BookCopies 
                          SET CopyNumber = {0}, 
                              Barcode = {1},
                              UpdatedAt = {2}
                          WHERE CopyId = {3}",
                        newCopyNumber, newBarcode, DateTime.Now, nullCopy.CopyId);
                    
                    // Kiểm tra xem có update được không
                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException($"Không thể cập nhật BookCopy với CopyId {nullCopy.CopyId}. Record có thể không tồn tại.");
                    }
                }
            }

            var bookImport = new BookImport
            {
                BookId = bookId,
                Quantity = quantity,
                ImportDate = DateTime.Now,
                ImportedBy = importedBy,
                CreatedAt = DateTime.Now
            };
            await _bookImportRepository.AddAsync(bookImport);

            // Lấy tất cả BookCopies hiện có của BookId này để tính sequence
            var allExistingCopies = await _context.BookCopies
                .Where(bc => bc.BookId == bookId)
                .ToListAsync();

            // Tìm max sequence number từ các CopyNumber hiện có
            // Format: B{BookId}-{sequence}
            var maxSequence = 0;
            var copyNumberPrefix = $"B{bookId}-";
            
            if (allExistingCopies.Any())
            {
                var sequences = allExistingCopies
                    .Where(bc => !string.IsNullOrWhiteSpace(bc.CopyNumber))
                    .Select(bc =>
                    {
                        var trimmed = bc.CopyNumber.Trim();
                        // Kiểm tra format B{BookId}-{sequence}
                        if (trimmed.StartsWith(copyNumberPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            var sequenceStr = trimmed.Substring(copyNumberPrefix.Length);
                            if (int.TryParse(sequenceStr, out var seq))
                            {
                                return seq;
                            }
                        }
                        // Kiểm tra format cũ B{sequence} (backward compatibility)
                        else if (trimmed.StartsWith("B", StringComparison.OrdinalIgnoreCase))
                        {
                            var numberStr = trimmed.Substring(1).Replace("-", "");
                            if (int.TryParse(numberStr, out var num))
                            {
                                return num;
                            }
                        }
                        return 0;
                    })
                    .Where(seq => seq > 0)
                    .ToList();

                if (sequences.Any())
                {
                    maxSequence = sequences.Max();
                }
            }

            // Lấy max Barcode sequence để tạo Barcode unique
            // Format Barcode: BC-{BookId}-{sequence} (VD: BC-3-001, BC-3-002)
            var maxBarcodeSequence = 0;
            var barcodePrefix = $"BC-{bookId}-";
            
            if (allExistingCopies.Any())
            {
                var barcodeSequences = allExistingCopies
                    .Where(bc => !string.IsNullOrWhiteSpace(bc.Barcode))
                    .Select(bc =>
                    {
                        var trimmed = bc.Barcode!.Trim();
                        // Kiểm tra format BC-{BookId}-{sequence}
                        if (trimmed.StartsWith(barcodePrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            var sequenceStr = trimmed.Substring(barcodePrefix.Length);
                            if (int.TryParse(sequenceStr, out var seq))
                            {
                                return seq;
                            }
                        }
                        return 0;
                    })
                    .Where(seq => seq > 0)
                    .ToList();

                if (barcodeSequences.Any())
                {
                    maxBarcodeSequence = barcodeSequences.Max();
                }
            }

            // Lấy danh sách CopyNumber và Barcode hiện có
            var existingCopyNumbersSet = allExistingCopies
                .Where(bc => !string.IsNullOrWhiteSpace(bc.CopyNumber))
                .Select(bc => bc.CopyNumber.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var existingBarcodesSet = allExistingCopies
                .Where(bc => !string.IsNullOrWhiteSpace(bc.Barcode))
                .Select(bc => bc.Barcode!.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var copies = new List<BookCopy>();

            for (int i = 1; i <= quantity; i++)
            {
                // Tạo CopyNumber với format: B{BookId}-{sequence}
                var sequence = maxSequence + i;
                var copyNumber = $"{copyNumberPrefix}{sequence:D3}";
                
                // Đảm bảo CopyNumber không trùng lặp
                int attempt = 0;
                while (existingCopyNumbersSet.Contains(copyNumber))
                {
                    sequence++;
                    copyNumber = $"{copyNumberPrefix}{sequence:D3}";
                    attempt++;
                    if (attempt > 1000)
                    {
                        throw new InvalidOperationException($"Không thể tạo CopyNumber duy nhất cho BookId {bookId}");
                    }
                }
                existingCopyNumbersSet.Add(copyNumber);

                // Tạo Barcode unique với format: BC-{BookId}-{sequence}
                var barcodeSequence = maxBarcodeSequence + i;
                var barcode = $"{barcodePrefix}{barcodeSequence:D3}";
                
                // Đảm bảo Barcode không trùng lặp
                attempt = 0;
                while (existingBarcodesSet.Contains(barcode))
                {
                    barcodeSequence++;
                    barcode = $"{barcodePrefix}{barcodeSequence:D3}";
                    attempt++;
                    if (attempt > 1000)
                    {
                        throw new InvalidOperationException($"Không thể tạo Barcode duy nhất cho BookId {bookId}");
                    }
                }
                existingBarcodesSet.Add(barcode);
                
                copies.Add(new BookCopy
                {
                    BookId = bookId,
                    CopyNumber = copyNumber,
                    Barcode = barcode, // Tạo Barcode unique để tránh unique constraint violation
                    Status = "Available",
                    Condition = "Good",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }

            await _bookCopyRepository.AddRangeAsync(copies);

            book.TotalCopies += quantity;
            book.AvailableCopies += quantity;
            await _bookRepository.UpdateAsync(book);

            return true;
        }

        public async Task<bool> AddAuthorToBookAsync(int bookId, int authorId, bool isPrimary = true)
        {
            var existing = await _bookAuthorRepository.FirstOrDefaultAsync(
                ba => ba.BookId == bookId && ba.AuthorId == authorId);
            
            if (existing != null) return false;

            var bookAuthor = new BookAuthor
            {
                BookId = bookId,
                AuthorId = authorId,
                IsPrimary = isPrimary
            };

            await _bookAuthorRepository.AddAsync(bookAuthor);
            return true;
        }

        public async Task<IEnumerable<BookCopy>> GetAvailableCopiesAsync(int bookId)
        {
            try
            {
                var allCopies = await _context.BookCopies
                    .Include(c => c.Book)
                    .Where(c => c.BookId == bookId)
                    .ToListAsync();
                
                System.Diagnostics.Debug.WriteLine($"GetAvailableCopiesAsync - BookId: {bookId}, Total copies: {allCopies.Count}");
                foreach (var copy in allCopies.Take(5))
                {
                    System.Diagnostics.Debug.WriteLine($"  CopyId: {copy.CopyId}, CopyNumber: {copy.CopyNumber}, Status: '{copy.Status}'");
                }
                
                var availableCopies = allCopies.Where(c => 
                {
                    var status = c.Status?.Trim();
                    var isAvailable = string.IsNullOrEmpty(status) || status.Equals("Available", StringComparison.OrdinalIgnoreCase);
                    if (!isAvailable)
                    {
                        System.Diagnostics.Debug.WriteLine($"  Copy {c.CopyId} ({c.CopyNumber}) is not available. Status: '{c.Status}'");
                    }
                    return isAvailable;
                }).ToList();
                
                System.Diagnostics.Debug.WriteLine($"GetAvailableCopiesAsync - Available copies: {availableCopies.Count}");
                
                // Nếu không có bản Available, vẫn trả về tất cả để hiển thị (có thể đang mượn hoặc trạng thái khác)
                // Người dùng sẽ thấy và có thể báo cáo nếu cần
                if (availableCopies.Count == 0 && allCopies.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: No Available copies found, but {allCopies.Count} total copies exist. Returning all copies.");
                    return allCopies;
                }
                
                return availableCopies;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAvailableCopiesAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private static bool IsCopyAvailable(BookCopy copy)
        {
            var status = copy.Status?.Trim();
            return string.IsNullOrEmpty(status) || status.Equals("Available", StringComparison.OrdinalIgnoreCase);
        }
    }
}

