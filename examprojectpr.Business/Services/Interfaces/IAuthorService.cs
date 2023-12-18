using examprojectpr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Interfaces
{
    public interface IAuthorService
    {
        Task CreateAsync(Author entity);
        Task Delete(int id);

        Task<List<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task UpdateAsync(Author entity);
    }
}
