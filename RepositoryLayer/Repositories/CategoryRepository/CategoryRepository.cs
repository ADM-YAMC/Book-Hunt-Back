using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.CategoryRepository
{
    public class CategoryRepository: ICommonProcess<Category>
    {
        private readonly BookHuntDBContext _context;
        public CategoryRepository(BookHuntDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.ToListAsync();

        public async Task<Category> GetByIdAsync(int id)
            =>  await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
       
        public async Task<(bool IsSuccess, string Message)> AddAsync(Category entry)
        {
            var exists = _context.Categories.Any(c=>c.Name == entry.Name);
            if (exists)
            {
                return (false, "Ya se ecuentra una categoría agregada con este nombre...");
            }
            await _context.Categories.AddAsync(entry);
            await _context.SaveChangesAsync();
            return (true, "La categoría ha sido agregada correctamente...");
        }


        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Category entry)
        {
            var exists = _context.Categories.Any(c => c.Name == entry.Name && c.CategoryId != entry.CategoryId);
            if (exists)
            {
                return (false, "Ya se ecuentra una categoría agregada con este nombre...");
            }
            _context.Categories.Update(entry);
            await _context.SaveChangesAsync();
            return (true, "La categoría ha sido actualizada correctamente...");
        }

        public async Task DeleteAsync(int id)
        {

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentOutOfRangeException("entity");
            }
        }
    }
}
