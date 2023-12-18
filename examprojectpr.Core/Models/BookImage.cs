using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Core.Models
{
    public class BookImage:BaseEntity
    {
        
        
        public string ImageUrl { get; set; }
        public bool? IsPoster { get; set; }  //null 3 cu hal ucundur
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
