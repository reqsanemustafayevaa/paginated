using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Core.Models
{
    public class Author:BaseEntity  //author-book one to many
    {
        public string FullName { get; set; }
        public List<Book>? Books { get; set; }
    }
}
