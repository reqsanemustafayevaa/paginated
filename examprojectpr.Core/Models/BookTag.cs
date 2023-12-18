using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Core.Models
{
    public class BookTag:BaseEntity
    {
        public int TagId { get; set; }
        public int BookId { get; set; }
        public Tag Tag { get; set; }
        public Book Book { get; set; }
    }
}
