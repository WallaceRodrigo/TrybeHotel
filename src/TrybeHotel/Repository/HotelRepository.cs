using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels.Select(h => new HotelDto()
            {
                HotelId = h.HotelId,
                Name = h.Name,
                Address = h.Address,
                CityId = h.CityId,
                CityName = _context.Cities.Where(c => c.CityId == h.CityId).Select(c => c.Name).FirstOrDefault(),
                State = _context.Cities.Where(c => c.CityId == h.CityId).Select(c => c.State).FirstOrDefault(),
            }).ToList();
        }

        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            var lastHotel = this.GetHotels();

            _context.Hotels.Add(new Hotel()
            {
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
            });

            _context.SaveChanges();

            return new HotelDto()
            {
                HotelId = lastHotel.Last().HotelId += 1,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = _context.Cities.Where(c => c.CityId == hotel.CityId).Select(c => c.Name).FirstOrDefault(),
                State = _context.Cities.Where(c => c.CityId == hotel.CityId).Select(c => c.State).FirstOrDefault(),
            };
        }
    }
}