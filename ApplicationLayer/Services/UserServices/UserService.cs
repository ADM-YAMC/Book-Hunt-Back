using DomainLayer.DTO;
using DomainLayer.Models;
using DomainLayer.Utilities;
using InfrastructureLayer.Repositories.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.UserServices
{
    public class UserService
    {
        private readonly ICommonProcess<User> _commonProcess;

        public UserService(ICommonProcess<User> commonProcess)
        {
            _commonProcess = commonProcess;
        }

        public async Task<Response<User>> GetUsersAsync()
        {
            var response = new Response<User>();
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

        public async Task<Response<User>> GetUserByIdAsync(int id)
        {
            var response = new Response<User>();
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
        public async Task<Response<string>> AddUserAsync(User user)
        {
            var response = new Response<string>();
            try
            {
                user.Password = Encode.MD5(user.Password);
              var result = await _commonProcess.AddAsync(user);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> UpdateUserAsync(int id, User user)
        {
            var response = new Response<string>();
            try
            {

                var result =  await _commonProcess.UpdateAsync(user);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;

            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteUserAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                await _commonProcess.DeleteAsync(id);   
                response.Message = "El usuario eliminado correctamente...";

            }
            catch (Exception e)
            {
                response.Message = "No se encontro el usuario...";
                response.Errors.Add(e.Message);
            }
            return response;
        }
    }
}
