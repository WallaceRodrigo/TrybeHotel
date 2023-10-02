using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            var user = _context.Users.Where(u => u.UserId == userId).Select(u => new UserDto()
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            }).FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.Where(u => u.Email == login.Email).Select(u => new User()
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType,
                Password = u.Password
            }).FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            if (user.Email != login.Email || user.Password != login.Password)
            {
                throw new Exception("Incorrect e-mail or password");
            }

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            try
            {
                _context.Users.Add(new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    UserType = "client"
                });
                _context.SaveChanges();

                var lastUser = _context.Users.OrderByDescending(u => u.UserId).ToList();

                return new UserDto
                {
                    UserId = lastUser.First().UserId,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = "client"
                };
            }
            catch (Exception)
            {

                throw new Exception("Erro ao adicionar usuário");
            }
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.Where(u => u.Email == userEmail).Select(u => new UserDto()
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            }).FirstOrDefault();

            if (user == null)
            {
                return null!;
            }

            return user;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users.Select(u => new UserDto()
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            }).ToList();
        }

    }
}