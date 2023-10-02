using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var Room = GetRoomById(booking.RoomId);

            if (Room.Capacity < booking.GuestQuant) throw new Exception("Guest quantity over room capacity");

            _context.Bookings.Add(new Booking()
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                UserId = _context.Users.Where(u => u.Email == email).Select(u => u.UserId).FirstOrDefault(),
                RoomId = booking.RoomId,
            });
            _context.SaveChanges();

            var lastBooking = _context.Bookings.OrderByDescending(b => b.BookingId).ToList();

            return new BookingResponse()
            {
                BookingId = lastBooking.First().BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = Room
            };
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            var Booking =  _context.Bookings.Where(b => b.BookingId == bookingId && b.UserId == user!.UserId).Select(b => new BookingResponse()
            {
                BookingId = b.BookingId,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                GuestQuant = b.GuestQuant,
                Room = GetRoomById(b.RoomId)
            }).ToList().FirstOrDefault()!;

            if (Booking == null)
            {
                throw new Exception("Booking not found");
            }

            return Booking;
        }

        public RoomDto GetRoomById(int RoomId)
        {
            return _context.Rooms.Where(r => r.RoomId == RoomId).Select(h => new RoomDto()
            {
                RoomId = h.RoomId,
                Name = h.Name,
                Capacity = h.Capacity,
                Image = h.Image,
                Hotel = new HotelDto()
                {
                #nullable disable
                    HotelId = h.Hotel.HotelId,
                    Name = h.Hotel.Name,
                    Address = h.Hotel.Address,
                    CityId = h.Hotel.CityId,
                    CityName = h.Hotel.City.Name,
                    State = _context.Cities.Where(c => c.CityId == h.Hotel.CityId).Select(c => c.State).FirstOrDefault(),
                #nullable enable
                }
            }).ToList().FirstOrDefault()!;
        }

        // public Room GetRoomById(int RoomId)
        // {
        //     return _context.Rooms.Where(r => r.RoomId == RoomId).Select(r => new Room()
        //     {
        //         RoomId = r.RoomId,
        //         Name = r.Name,
        //         Capacity = r.Capacity,
        //         Image = r.Image,
        //         Hotel = r.Hotel
        //     }).ToList().FirstOrDefault()!;
        // }

    }

}