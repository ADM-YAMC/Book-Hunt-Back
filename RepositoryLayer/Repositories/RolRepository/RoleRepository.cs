using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.RolRepository
{
    public class RoleRepository : ICommonProcess<Role>
    {
        private readonly BookHuntDBContext _context;
        public RoleRepository(BookHuntDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
            => await _context.Roles.ToListAsync();

        public async Task<Role> GetByIdAsync(int id)
            => await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(Role entry)
        {
            var exists = _context.Roles.Any(r => r.RoleName == entry.RoleName);
            if (exists)
            {
                return (false, "Ya se ecuentra un rol registrado con este mismo nombre...");
            }
            await _context.Roles.AddAsync(entry);
            await _context.SaveChangesAsync();
            return (true, "Se guardo la información del rol correctamente...");
        }


        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Role entry)
        {
            var exists = _context.Roles.Any(r => r.RoleName == entry.RoleName && r.RoleId != entry.RoleId);
            if (exists)
            {
                return (false, "Ya se ecuentra un rol registrado con este mismo nombre...");
            }
            _context.Roles.Update(entry);
            await _context.SaveChangesAsync();
            return (true, "Se actualizo la información del rol correctamente...");
        }

        public async Task DeleteAsync(int id)
        {

            var data = await _context.Roles.FindAsync(id);
            if (data != null)
            {
                _context.Roles.Remove(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentOutOfRangeException("entity");
            }
        }
    }
}
