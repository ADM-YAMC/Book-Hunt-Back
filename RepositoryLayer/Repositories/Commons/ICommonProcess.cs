using DomainLayer.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories.Commons
{
    public interface ICommonProcess<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<(bool IsSuccess, string Message)> AddAsync(T entry);
        Task<(bool IsSuccess, string Message)> UpdateAsync(T entry);
        Task DeleteAsync(int id);
    }
}
