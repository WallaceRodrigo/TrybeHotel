using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities.Select(c => new CityDto()
            {
                CityId = c.CityId,
                Name = c.Name,
                State = c.State,
            }).ToList();
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            var lastCity = this.GetCities();

            _context.Cities.Add(new City()
            {
                Name = city.Name,
                State = city.State,
            });

            _context.SaveChanges();

            return new CityDto()
            {
                CityId = (lastCity.Count() == 0) ? 1 : lastCity.Last().CityId + 1,
                Name = city.Name,
                State = city.State,
            };
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var updatedCity = _context.Cities.Update(city);

            _context.SaveChanges();

            if (updatedCity == null) throw new Exception("City not found");

            return _context.Cities.Where(c => c.CityId == city.CityId)
            .Select(c => new CityDto()
            {
                CityId = c.CityId,
                Name = c.Name,
                State = c.State
            }).FirstOrDefault()!;
        }
    }
}