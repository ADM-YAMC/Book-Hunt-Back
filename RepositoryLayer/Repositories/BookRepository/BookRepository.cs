using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.BookRepository
{
    public class BookRepository :  IBook
    {
        private readonly BookHuntDBContext _context;
        public BookRepository(BookHuntDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookDto>> GetBookAsync()
               => await _context
                    .Books
                    .Include(b => b.Categories)
                    .ThenInclude(bc => bc.Category)
                    .Include(b => b.Authors)
                    .ThenInclude(ba => ba.Author)
                    .Select(b => new BookDto
                    {
                        BookId = b.BookId,
                        Title = b.Title,
                        Description = b.Description,
                        PublicationDate = b.PublicationDate,
                        UrlImageBook = b.UrlImageBook,
                        IsActive = b.IsActive,
                        Authors = b.Authors.Select(ba => new AuthorDto
                        {
                            AuthorId = ba.Author.AuthorId,
                            Name = $"{ba.Author.Name} {ba.Author.LastName}",
                            IsActive = ba.Author.IsActive,
                        }).ToList(),
                        Categories = b.Categories.Select(bc=> new CategoryDto
                        {
                            CategoryId = bc.Category.CategoryId,
                            Name = bc.Category.Name,
                            IsActive = bc.Category.IsActive,
                        }).ToList(),
                    })
                    .ToListAsync();

        public async Task<BookDto> GetBookAsync(int id)
        {
            var book  = await _context
                    .Books
                    .Include(b => b.Categories)
                    .ThenInclude(bc => bc.Category)
                    .Include(b => b.Authors)
                    .ThenInclude(ba => ba.Author)
                    .Select(b => new BookDto
                    {
                        BookId = b.BookId,
                        Title = b.Title,
                        Description = b.Description,
                        PublicationDate = b.PublicationDate,
                        UrlImageBook = b.UrlImageBook,
                        IsActive = b.IsActive,
                        Authors = b.Authors.Select(ba => new AuthorDto
                        {
                            AuthorId = ba.Author.AuthorId,
                            Name = $"{ba.Author.Name} {ba.Author.LastName}",
                            IsActive = ba.Author.IsActive,
                        }).ToList(),
                        Categories = b.Categories.Select(bc => new CategoryDto
                        {
                            CategoryId = bc.Category.CategoryId,
                            Name = bc.Category.Name,
                            IsActive = bc.Category.IsActive,
                        }).ToList(),
                    })
                    .FirstOrDefaultAsync(x => x.BookId == id);

            return book!;

        }
        public async Task<(bool IsSuccess, string Message, Book Book)> AddBookAsync(BookCreateDto bookDto)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                PublicationDate = bookDto.PublicationDate,
                IsActive = bookDto.IsActive,
                UrlImageBook = bookDto.UrlImageBook,
                Authors = bookDto.AuthorIds.Select(id => new BookAuthor { AuthorId = id, IsActive = bookDto.IsActive }).ToList(),
                Categories = bookDto.CategoryIds.Select(id => new BookCategory { CategoryId = id, IsActive = bookDto.IsActive }).ToList(),
            };

            var exict = _context.Books.Any(b => b.Title == bookDto.Title);

            if (!exict)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }

            return (true, exict ? "Ya existe un libre con este mismo título...": "Libro creado correctamente...", book);
        }
        
        public async Task<(bool IsSuccess, string Message)> UpdateBookAsync(int id, BookCreateDto bookDto)
        {
           
            var book = await _context.Books
                .Include(b => b.Authors)
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.BookId == id);

                if (book == null)
                {
                    return (false, "No se encontró el libro...");
                }

                var exict = _context.Books.Any(b => b.Title == bookDto.Title && b.BookId != id);

                if (exict)
                {
                    return (false, "Ya existe un libre con este mismo título...");
                }

                book.Title = bookDto.Title;
                book.Description = bookDto.Description;
                book.PublicationDate = bookDto.PublicationDate;
                book.IsActive = bookDto.IsActive;
                book.UrlImageBook = bookDto.UrlImageBook;
                var temporalListBookAuthors = new List<BookAuthor>();

                var autoresEliminados = book.Authors
                    .Where(ba => !bookDto.AuthorIds.Contains(ba.AuthorId))
                    .ToList();

                autoresEliminados.ForEach(ae => {
                    if (!temporalListBookAuthors.Any(x => x.AuthorId == ae.AuthorId))
                    {
                        temporalListBookAuthors.Add(ae);
                    }
                });

                var autoresAgregados = bookDto.AuthorIds
                    .Where(id => !book.Authors.Any(ba => ba.AuthorId == id))
                    .Select(id => new BookAuthor { AuthorId = id, BookId = book.BookId, IsActive = bookDto.IsActive })
                    .ToList();

                if (autoresEliminados.Count != 0)
                {
                    foreach (var author in temporalListBookAuthors)
                    {
                        _context.Entry(author).State = EntityState.Deleted;
                    }
                }

                if (autoresAgregados.Count != 0)
                {
                    foreach (var author in autoresAgregados)
                    {
                        _context.Entry(author).State = EntityState.Added;
                    }
                }
                var temporalListBookCategories = new List<BookCategory>();

                var categoriasEliminadas = book.Categories
                    .Where(bc => !bookDto.CategoryIds.Contains(bc.CategoryId))
                    .ToList();

                categoriasEliminadas.ForEach(ce => {
                    if (!temporalListBookCategories.Any(x => x.CategoryId == ce.CategoryId))
                    {
                        temporalListBookCategories.Add(ce);
                    }
                });

                var categoriasAgregadas = bookDto.CategoryIds
                    .Where(id => !book.Categories.Any(bc => bc.CategoryId == id))
                    .Select(id => new BookCategory { CategoryId = id, BookId = book.BookId, IsActive = bookDto.IsActive })
                    .ToList();

                if (categoriasEliminadas.Count != 0)
                {
                    foreach (var category in temporalListBookCategories)
                    {
                        _context.Entry(category).State = EntityState.Deleted;
                    }
                }
                if (categoriasAgregadas.Count != 0)
                {
                    foreach (var category in categoriasAgregadas)
                    {
                        _context.Entry(category).State = EntityState.Added;
                    }
                }
                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return (true, "Libro actualizado correctamente...");

        }

        public async Task<(bool IsSuccess, string Message)> DeleteBookAsync(int id)
        {
            var data = await _context.Books.FindAsync(id);
            if (data != null)
            {
                _context.Books.Remove(data);
                await _context.SaveChangesAsync();
                return (true, "Libro actualizado correctamente...");
            }
            else
            {
                return (false, "No se pudo eliminar el libro porque no fue encontrado...");
            }
        }
    }
}
