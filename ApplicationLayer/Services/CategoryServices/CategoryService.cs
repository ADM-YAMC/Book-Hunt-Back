using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.CategoryServices
{
    public class CategoryService
    {
        private readonly ICommonProcess<Category> _commonProcess;

        public CategoryService(ICommonProcess<Category> commonProcess)
        {
            _commonProcess = commonProcess;
        }

        public async Task<Response<Category>> GetCategoryAsync()
        {
            var response = new Response<Category>();
            try
            {
                response.DataList = await _commonProcess.GetAllAsync();
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<Category>> GetCategoryByIdAsync(int id)
        {
            var response = new Response<Category>();
            try
            {
                var res = await _commonProcess.GetByIdAsync(id);
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
        public async Task<Response<string>> AddCategoryAsync(Category entry)
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

        public async Task<Response<string>> UpdateCategoryAsync(int id, Category entry)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonProcess.UpdateAsync(entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteCategoryAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                await _commonProcess.DeleteAsync(id);
                response.Message = "Se elimino la información de la categoría correctamente......";

            }
            catch (Exception e)
            {
                response.Message = "No se encontro la categoría...";
                response.Errors.Add(e.Message);
            }
            return response;
        }
    }
}
