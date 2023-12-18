using examprojectpr.Core.Models;
using examprojectpr.Core.Repostories.Interfaces;
using examprojectpr.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Data.Repostories.Implementations
{
    public class BookRepository : GenericRepostory<Book>, IBookRepository
    {
        public BookRepository(AppDbcontext context) : base(context)
        {
        }
    }
}
