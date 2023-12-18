using examprojectpr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Interfaces
{
    public interface ITagService
    {
        Task CreateAsync(Tag entity);
        Task Delete(int id);
        Task<Tag> GetByIdAsync(int id);
        Task<List<Tag>> GetAllAsync();
        Task UpdateAsync(Tag entity);
    }
}
