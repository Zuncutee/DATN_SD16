using DATN_SD16.Models.Entities;
using DATN_SD16.Repositories.Interfaces;
using DATN_SD16.Services.Interfaces;
using System.Text;

namespace DATN_SD16.Services
{
    public class ReadingRoomService : IReadingRoomService
    {
        private readonly IReadingRoomRepository _roomRepository;
        private readonly IRepository<ReadingRoomSeat> _seatRepository;
        private readonly IRepository<ReadingRoomReservation> _reservationRepository;

        public ReadingRoomService(
            IReadingRoomRepository roomRepository,
            IRepository<ReadingRoomSeat> seatRepository,
            IRepository<ReadingRoomReservation> reservationRepository)
        {
            _roomRepository = roomRepository;
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
        }

        #region Room Management
        public async Task<ReadingRoom?> GetRoomByIdAsync(int roomId)
        {
            return await _roomRepository.GetByIdAsync(roomId);
        }

        public async Task<IEnumerable<ReadingRoom>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ReadingRoom>> GetActiveRoomsAsync()
        {
            return await _roomRepository.GetActiveRoomsAsync();
        }

        public async Task<ReadingRoom?> GetRoomWithSeatsAsync(int roomId)
        {
            return await _roomRepository.GetRoomWithSeatsAsync(roomId);
        }

        public async Task<ReadingRoom> CreateRoomAsync(ReadingRoom room, int createdBy)
        {
            room.IsActive = true;
            room.CreatedAt = DateTime.Now;
            room.UpdatedAt = DateTime.Now;
            return await _roomRepository.AddAsync(room);
        }

        public async Task<bool> UpdateRoomAsync(ReadingRoom room)
        {
            var existing = await _roomRepository.GetByIdAsync(room.RoomId);
            if (existing == null) return false;

            existing.RoomName = room.RoomName;
            existing.RoomCode = room.RoomCode;
            existing.Capacity = room.Capacity;
            existing.Description = room.Description;
            existing.IsActive = room.IsActive;
            existing.UpdatedAt = DateTime.Now;

            await _roomRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) return false;

            room.IsActive = false;
            room.UpdatedAt = DateTime.Now;
            await _roomRepository.UpdateAsync(room);
            return true;
        }
        #endregion

        #region Seat Management
        public async Task<ReadingRoomSeat?> GetSeatByIdAsync(int seatId)
        {
            return await _seatRepository.GetByIdAsync(seatId);
        }

        public async Task<IEnumerable<ReadingRoomSeat>> GetSeatsByRoomIdAsync(int roomId)
        {
            return await _seatRepository.FindAsync(s => s.RoomId == roomId);
        }

        public async Task<IEnumerable<ReadingRoomSeat>> GetAvailableSeatsAsync(int roomId)
        {
            return await _seatRepository.FindAsync(s => s.RoomId == roomId && s.Status == "Available");
        }

        public async Task<ReadingRoomSeat> CreateSeatAsync(ReadingRoomSeat seat)
        {
            seat.Status = "Available";
            seat.CreatedAt = DateTime.Now;
            seat.UpdatedAt = DateTime.Now;
            
            seat.QRCode = await GenerateQRCodeForSeatAsync(0);
            
            var created = await _seatRepository.AddAsync(seat);
            
            created.QRCode = $"SEAT-{created.SeatId}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
            await _seatRepository.UpdateAsync(created);
            
            return created;
        }

        public async Task<bool> UpdateSeatAsync(ReadingRoomSeat seat)
        {
            var existing = await _seatRepository.GetByIdAsync(seat.SeatId);
            if (existing == null) return false;

            existing.SeatNumber = seat.SeatNumber;
            existing.Status = seat.Status;
            if (!string.IsNullOrEmpty(seat.QRCode))
            {
                existing.QRCode = seat.QRCode;
            }
            existing.UpdatedAt = DateTime.Now;

            await _seatRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteSeatAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat == null) return false;

            await _seatRepository.DeleteAsync(seat);
            return true;
        }

        public async Task<string> GenerateQRCodeForSeatAsync(int seatId)
        {
            return await Task.FromResult($"SEAT-{seatId}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}");
        }
        #endregion

        #region Reservation Management
        public async Task<ReadingRoomReservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _reservationRepository.GetByIdAsync(reservationId);
        }

        public async Task<IEnumerable<ReadingRoomReservation>> GetReservationsByUserIdAsync(int userId)
        {
            return await _reservationRepository.FindAsync(r => r.UserId == userId);
        }

        public async Task<IEnumerable<ReadingRoomReservation>> GetActiveReservationsAsync()
        {
            var now = DateTime.Now;
            return await _reservationRepository.FindAsync(r => 
                (r.Status == "Reserved" || r.Status == "CheckedIn") &&
                r.ReservationDate.Date == now.Date);
        }

        public async Task<ReadingRoomReservation> CreateReservationAsync(int userId, int seatId, DateTime reservationDate, TimeSpan startTime, TimeSpan endTime)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat == null || seat.Status != "Available")
            {
                throw new Exception("Chỗ ngồi không khả dụng");
            }

            var existingReservations = await _reservationRepository.FindAsync(r =>
                r.SeatId == seatId &&
                r.ReservationDate.Date == reservationDate.Date &&
                (r.Status == "Reserved" || r.Status == "CheckedIn"));

            foreach (var existing in existingReservations)
            {
                if ((startTime >= existing.StartTime && startTime < existing.EndTime) ||
                    (endTime > existing.StartTime && endTime <= existing.EndTime) ||
                    (startTime <= existing.StartTime && endTime >= existing.EndTime))
                {
                    throw new Exception("Chỗ ngồi đã được đặt trong khung giờ này");
                }
            }

            var reservation = new ReadingRoomReservation
            {
                UserId = userId,
                SeatId = seatId,
                ReservationDate = reservationDate,
                StartTime = startTime,
                EndTime = endTime,
                Status = "Reserved",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            seat.Status = "Reserved";
            seat.UpdatedAt = DateTime.Now;
            await _seatRepository.UpdateAsync(seat);

            return await _reservationRepository.AddAsync(reservation);
        }

        public async Task<bool> CheckInAsync(int reservationId, string qrCode)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "Reserved")
            {
                return false;
            }

            var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
            if (seat == null || seat.QRCode != qrCode)
            {
                return false;
            }

            reservation.CheckInTime = DateTime.Now;
            reservation.Status = "CheckedIn";
            await _reservationRepository.UpdateAsync(reservation);

            seat.Status = "Occupied";
            seat.UpdatedAt = DateTime.Now;
            await _seatRepository.UpdateAsync(seat);

            return true;
        }

        public async Task<bool> CheckOutAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != "CheckedIn")
            {
                return false;
            }

            var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
            if (seat == null)
            {
                return false;
            }

            reservation.CheckOutTime = DateTime.Now;
            reservation.Status = "Completed";
            await _reservationRepository.UpdateAsync(reservation);

            seat.Status = "Available";
            seat.UpdatedAt = DateTime.Now;
            await _seatRepository.UpdateAsync(seat);

            return true;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status == "Completed" || reservation.Status == "Cancelled")
            {
                return false;
            }

            var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
            if (seat != null)
            {
                seat.Status = "Available";
                seat.UpdatedAt = DateTime.Now;
                await _seatRepository.UpdateAsync(seat);
            }

            reservation.Status = "Cancelled";
            await _reservationRepository.UpdateAsync(reservation);

            return true;
        }
        #endregion
    }
}
