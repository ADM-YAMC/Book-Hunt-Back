using ApplicationLayer.Services.UserServices;
using DomainLayer.DTO;
using DomainLayer.Utilities;
using DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using InfrastructureLayer.Repositories.UserRepository;

namespace ApplicationLayer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUser _userService;
        public AuthService(IConfiguration configuration, IUser userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<Response<AuthAccount>> Authenticate(UserCredentials credentials)
        {
            var response = new Response<AuthAccount>(); 
            try
            {
                 var user = await _userService.Login(credentials.Email,credentials.Password);
                
                if (user.IsSuccess)
                {
                    var dataUser = user.user;
                    response.SingleData = new AuthAccount
                    {
                        Id = dataUser!.Id,
                        Name = dataUser.Name,
                        LastName = dataUser.LastName,
                        Email = dataUser.Email,
                        RoleId = dataUser.RoleId,
                        RoleName = dataUser.Role!.RoleName,
                        IsActive = dataUser.IsActive,
                        Token = GenerateJwtToken(dataUser),
                    };
                    response.Successful = true; 
                }
                else
                {
                    response.Successful = false;
                    response.Message = user.Message;
                }

            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
                response.Message = "Ocurrió un error al intentar iniciar sesión...";
            }

            return response;
        }

        public async Task<Response<string>> RefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var response = new Response<string>();  
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero 
                }, out SecurityToken validatedToken);

              
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "Id").Value;

                var user = new User 
                {
                    Id = int.Parse(userId),
                    Name = jwtToken.Claims.First(x => x.Type == "Name").Value,
                    LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value,
                    Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
                    RoleId = int.Parse(jwtToken.Claims.First(x => x.Type == "RoleId").Value),
                    Role = new Role { RoleName = jwtToken.Claims.First(x => x.Type == "RoleName").Value },
                    IsActive = bool.Parse(jwtToken.Claims.First(x => x.Type == "IsActive").Value)
                };
                var newJwtToken = GenerateJwtToken(user);
                response.SingleData = newJwtToken;
                response.Successful = true;
                
            }
            catch (Exception ex) 
            {
                response.Errors.Add(ex.Message);
                response.Message = "No se pudo generar el token... Intentelo de nuevo.";
            }

            return response;
            
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //var claims = new[]
            //{
            //    new Claim("Id", user.Id.ToString()),
            //    new Claim("Name",user.Name),
            //    new Claim("LastName", user.LastName),
            //    new Claim("Email", user.Email),
            //    new Claim("RoleId", user.RoleId.ToString()),
            //    new Claim("RoleName", user.Role!.RoleName),
            //    new Claim("IsActive", user.IsActive.ToString())    
            //};
            //var token = new JwtSecurityToken(
            //        claims: claims,
            //        notBefore: DateTime.Now,
            //        expires: DateTime.Now.AddMinutes(2),
            //        signingCredentials: creds);

            //    return new JwtSecurityTokenHandler().WriteToken(token);


            var tokenHandler = new JwtSecurityTokenHandler();
            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name",user.Name),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email),
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim("RoleName", user.Role!.RoleName),
                    new Claim("IsActive", user.IsActive.ToString())
                }),
                NotBefore = DateTimeRD().currentDate,
                Expires = DateTimeRD().expires,
                SigningCredentials = creds
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public (DateTime currentDate, DateTime expires) DateTimeRD()
        {
            TimeZoneInfo dominicanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time");
            DateTime currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, dominicanTimeZone);
            var expirationHour =1;
            var expires = currentDate.AddMinutes((double)expirationHour);
            return (currentDate, expires);
        }

    }
}
