﻿
using ApplicationLayer.Services.BooksServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class BookController : ControllerBase
    {
        private readonly BookService _service;
        public BookController(BookService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<Response<BookDto>>> GetBookAsync()
            => await _service.GetBookAllAsync();
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BookDto>>> GetBookByIdAsync(int id)
            => await _service.GetBookByIdAsync(id);
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddBookAsync(BookCreateDto entry)
            => await _service.AddBookAsync(entry);
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Response<string>>> UpdateBookAsync(int id, BookCreateDto entry)
            => await _service.UpdateBookAsync(id, entry);
        [Authorize]

        [HttpDelete("{id}")]
        public async Task<Response<string>> DeleteBookAsync(int id)
            => await _service.DeleteBookAsync(id);
    }
}
