using DomainLayer.DTO;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.BookRepository
{
    public interface IBook
    {
        Task<(bool IsSuccess, string Message, Book Book)> AddBookAsync(BookCreateDto bookDto);
        Task<IEnumerable<BookDto>> GetBookAsync();
        Task<BookDto> GetBookAsync(int id);
        Task<(bool IsSuccess, string Message)> UpdateBookAsync(int id, BookCreateDto bookDto);
        Task<(bool IsSuccess, string Message)> DeleteBookAsync(int id);
    }
}
