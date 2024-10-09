using DomainLayer.Models;
using DomainLayer.Utilities;
using InfrastructureLayer;
using InfrastructureLayer.Repositories.Commons;
using InfrastructureLayer.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLayer.UserServices
{
    public class UsersRepository : ICommonProcess<User>, IUser
    {
        private readonly BookHuntDBContext _context;
        public UsersRepository(BookHuntDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context
                .Users
                .Include(u => u.Role)
                .Select(s => new User
                {
                    Id = s.Id,
                    Name = s.Name,
                    LastName = s.LastName,
                    Email = s.Email,
                    Role = s.Role,
                    IsActive = s.IsActive,
                    RoleId = s.RoleId,

                })
                .ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var res = await _context
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (res != null)
            {
                res.Password = null;
                return res;
            }
            else
            {
                return null!;
            }
        }
        public async Task<(bool IsSuccess, string Message)> AddAsync(User entry)
        {
            var exists = _context.Users.Any(c => c.Email == entry.Email);
            if (exists)
            {
                return (false, "Ya se ecuentra un usuario registrado con este mismo correo...");
            }
            await _context.Users.AddAsync(entry);
            await _context.SaveChangesAsync();
            return (true, "El usuario guardado correctamente...");
        }


        public async Task<(bool IsSuccess, string Message)> UpdateAsync(User entry)
        {
            var exists = _context.Users.Any(c => c.Email == entry.Email && c.Id != entry.Id);
            if (exists)
            {
                return (false, "Ya se encuentra un usuario registrado con este mismo correo...");
            }
            var userInDb = await _context.Users.FindAsync(entry.Id);
            if (userInDb == null)
            {
                return (false, "Usuario no encontrado...");
            }
            if (string.IsNullOrEmpty(entry.Password))
            {
                entry.Password = userInDb.Password;
            }
            else
            {
                userInDb.Password = Encode.MD5(entry.Password);
            }
            _context.Users.Update(entry);
            await _context.SaveChangesAsync();

            return (true, "El usuario actualizado correctamente...");
        }


        public async Task DeleteAsync(int id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentOutOfRangeException("entity");
            }
        }

        public async Task<(bool IsSuccess, string Message, User? user)> Login(string username, string password)
        {
            var user = await _context
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == username && u.Password == Encode.MD5(password));

            if (user == null)
            {
                return (false, "Los datos ingresados no son correctos...", null);
            }
            return (true, "", user);
        }
    }
}
