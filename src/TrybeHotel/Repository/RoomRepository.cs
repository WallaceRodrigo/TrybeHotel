using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotelById(int HotelId)
        {
            return _context.Hotels.Where(h => h.HotelId == HotelId).Select(h => new HotelDto()
            {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                CityId = h.CityId,
                CityName = _context.Cities.Where(c => c.CityId == h.CityId).Select(c => c.Name).FirstOrDefault(),
                State = _context.Cities.Where(c => c.CityId == h.CityId).Select(c => c.State).FirstOrDefault(),
            }).ToList();
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms.Select(r => new RoomDto()
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                Hotel = this.GetHotelById(HotelId).FirstOrDefault()
            }).ToList();
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            var lastRoom = this.GetRooms(room.HotelId);

            _context.Rooms.Add(new Room()
            {
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                HotelId = room.HotelId
            });

            _context.SaveChanges();

            int rId = (lastRoom.Count() == 0) ? 1 : lastRoom.Last().RoomId + 1;
            return new RoomDto()
            {
                RoomId = rId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = this.GetHotelById(room.HotelId).FirstOrDefault()
            };
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId)
        {
            var roomToDelete = _context.Rooms.FirstOrDefault(r => r.RoomId == RoomId);

            if (roomToDelete != null)
            {
                _context.Rooms.Remove(roomToDelete);
                _context.SaveChanges();
            }
        }
    }
}