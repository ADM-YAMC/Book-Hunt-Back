using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositories.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.RoleServices
{
    public class RoleService
    {
        private readonly ICommonProcess<Role> _commonProcess;

        public RoleService(ICommonProcess<Role> commonProcess)
        {
            _commonProcess = commonProcess;
        }

        public async Task<Response<Role>> GetRoleAllAsync()
        {
            var response = new Response<Role>();
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

        public async Task<Response<Role>> GetRoleByIdAsync(int id)
        {
            var response = new Response<Role>();
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
        public async Task<Response<string>> AddRoleAsync(Role entry)
        {
            var response = new Response<string>();
            try
            {
              var result =  await _commonProcess.AddAsync(entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> UpdateRoleAsync(int id, Role entry)
        {
            var response = new Response<string>();
            try
            {
                var result =  await _commonProcess.UpdateAsync(entry);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;
        }

        public async Task<Response<string>> DeleteRoleAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                await _commonProcess.DeleteAsync(id);
                response.Message = "Se elimino la información del rol correctamente......";

            }
            catch (Exception e)
            {
                response.Message = "No se encontro el rol...";
                response.Errors.Add(e.Message);
            }
            return response;
        }
    }
}
