using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.AuthorRepository
{
    public class AuthorRepository: ICommonProcess<Author>
    {
        private readonly BookHuntDBContext _context;
        public AuthorRepository(BookHuntDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAllAsync()
            => await _context.Authors.ToListAsync();

        public async Task<Author> GetByIdAsync(int id)
            => await _context.Authors.FirstOrDefaultAsync(x => x.AuthorId == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(Author entry)
        {
            var exists = _context.Authors.Any(a=>a.Name == entry.Name && a.LastName == entry.LastName);
            if (exists)
            {
                return (false, "Ya hay un autor con este mismo nombre y apellido...");
            }
            await _context.Authors.AddAsync(entry);
            await _context.SaveChangesAsync();
            return (true, "Se guardo la información del autor correctamente...");
            
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Author entry)
        {
            var exists = _context.Authors
                .Any(a => a.Name == entry.Name && a.LastName == entry.LastName && a.AuthorId != entry.AuthorId);

            if (exists)
            {
                return (false, "Ya hay un autor con este mismo nombre y apellido...");
            }
            _context.Authors.Update(entry);
            await _context.SaveChangesAsync();
            return (true, "Se actualizo la información del autor correctamente...");
        }


        public async Task DeleteAsync(int id)
        {

            var data = await _context.Authors.FindAsync(id);
            if (data != null)
            {
                _context.Authors.Remove(data);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentOutOfRangeException("entity");
            }
        }
    }
}
