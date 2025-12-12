using DATN_SD16.Data;
using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DATN_SD16.Repositories
{
    public class ReadingRoomRepository : Repository<ReadingRoom>, IReadingRoomRepository
    {
        public ReadingRoomRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReadingRoom>> GetActiveRoomsAsync()
        {
            return await _context.ReadingRooms
                .Include(r => r.ReadingRoomSeats)
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoomName)
                .ToListAsync();
        }

        public async Task<ReadingRoom?> GetRoomWithSeatsAsync(int roomId)
        {
            return await _context.ReadingRooms
                .Include(r => r.ReadingRoomSeats)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        public async Task<IEnumerable<ReadingRoom>> GetRoomsWithAvailableSeatsAsync()
        {
            return await _context.ReadingRooms
                .Include(r => r.ReadingRoomSeats)
                .Where(r => r.IsActive && r.ReadingRoomSeats.Any(s => s.Status == "Available"))
                .OrderBy(r => r.RoomName)
                .ToListAsync();
        }
    }
}

