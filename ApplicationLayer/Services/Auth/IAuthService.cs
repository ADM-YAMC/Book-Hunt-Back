using DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.Auth
{
    public interface IAuthService
    {
        Task<Response<AuthAccount>> Authenticate(UserCredentials credentials);
        Task<Response<string>> RefreshToken(string token);
    }
}
