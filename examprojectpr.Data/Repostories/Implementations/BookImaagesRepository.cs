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
    public class BookImaagesRepository : GenericRepostory<BookImage>, IBookImagesRepository
    {
        public BookImaagesRepository(AppDbcontext context) : base(context)
        {
        }
    }
}
