using DATN_SD16.Models.Entities;

namespace DATN_SD16.Repositories.Interfaces
{
    public interface IReadingRoomRepository : IRepository<ReadingRoom>
    {
        Task<IEnumerable<ReadingRoom>> GetActiveRoomsAsync();
        Task<ReadingRoom?> GetRoomWithSeatsAsync(int roomId);
        Task<IEnumerable<ReadingRoom>> GetRoomsWithAvailableSeatsAsync();
    }
}

