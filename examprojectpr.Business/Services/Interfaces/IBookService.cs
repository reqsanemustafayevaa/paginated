using examprojectpr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Business.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(Book entity);
        Task Delete(int id);
        Task SoftDelete(int id);  //butun columnlar ucun

        Task<List<Book>> GetAllAsync(Expression<Func<Book, bool>>? expression = null); Task<List<Book>> GetAllRelatedBooksAsync(Book book);
        Task<Book> GetByIdAsync(int id);
        Task UpdateAsync(Book entity);
    }
}
