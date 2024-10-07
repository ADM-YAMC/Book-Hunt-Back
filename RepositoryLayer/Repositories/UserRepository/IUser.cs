using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.UserRepository
{
    public interface IUser
    {
        Task<(bool IsSuccess, string Message,User? user)> Login(string username, string password);
    }
}
