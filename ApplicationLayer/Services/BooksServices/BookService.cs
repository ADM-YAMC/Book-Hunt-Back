using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositories.BookRepository;
using InfrastructureLayer.Repositories.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.BooksServices
{
    public class BookService
    {
        private readonly IBook _book;

        public BookService( IBook book)
        {
            _book = book;   
        }

        public async Task<Response<BookDto>> GetBookAllAsync()
        {
            var response = new Response<BookDto>();
            try
            {
                response.DataList = await _book.GetBookAsync();
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<BookDto>> GetBookByIdAsync(int id)
        {
            var response = new Response<BookDto>();
            try
            {
                var res = await _book.GetBookAsync(id);
                if (res != null)
                {
                    response.SingleData = res;
                }
                else
                {
                    response.Message = "No se encontro información...";
                }

            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }
        public async Task<Response<string>> AddBookAsync(BookCreateDto entry)
        {
            var response = new Response<string>();
            try
            {
                await _book.AddBookAsync(entry);
                response.Message = "Se guardo la información del libro correctamente...";
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> UpdateBookAsync(int id, BookCreateDto entry)
        {
            var response = new Response<string>();
            try
            {
               var result = await _book.UpdateBookAsync(id,entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteBookAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _book.DeleteBookAsync(id);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Message = "No se encontro el libro...";
                response.Errors.Add(e.Message);
            }
            return response;
        }
    }
}
