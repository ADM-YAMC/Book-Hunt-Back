using DomainLayer.DTO;
using DomainLayer.Models;
using DomainLayer.Utilities;
using InfrastructureLayer.Repositories.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.AuthorServices
{
    public class AuthorService
    {
        private readonly ICommonProcess<Author> _commonProcess;

        public AuthorService(ICommonProcess<Author> commonProcess)
        {
            _commonProcess = commonProcess;
        }

        public async Task<Response<Author>> GetAuthorAllAsync()
        {
            var response = new Response<Author>();
            try
            {
                response.DataList = await _commonProcess.GetAllAsync();
                response.Successful = true;
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<Author>> GetAuthorByIdAsync(int id)
        {
            var response = new Response<Author>();
            try
            {
                var res = await _commonProcess.GetByIdAsync(id);
                if (res != null)
                {
                    response.SingleData = res;
                    response.Successful = true;
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
        public async Task<Response<string>> AddAuthorAsync(Author entry)
        {
            var response = new Response<string>();
            try
            {
               var result = await _commonProcess.AddAsync(entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> UpdateAuthorAsync(int id, Author entry)
        {
            var response = new Response<string>();
            try
            {
                 var result =   await _commonProcess.UpdateAsync(entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteAuthorAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                await _commonProcess.DeleteAsync(id);
                response.Message = "Se elimino la información del autor correctamente...";
                response.Successful = true;
            }
            catch (Exception e)
            {
                response.Message = "No se encontro el autor...";
                response.Errors.Add(e.Message);
            }
            return response;
        }
    }
}
