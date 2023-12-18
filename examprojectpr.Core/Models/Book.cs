using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examprojectpr.Core.Models
{
    public class Book:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tax { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }   //stock
        public double CostPrice { get; set; }    //maya
        public double SalePrice { get; set; }    //satis
        public double DiscountPercent { get; set; }  //endirim
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSeller { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int AuthorId { get; set; }
        public Author? author { get; set; }

        public List<BookTag>? BookTags { get; set; }

        [NotMapped]
        public List<int> TagIds { get; set; }

        public List<BookImage>? BookImages { get; set; } //relation

        [NotMapped]
        public IFormFile? BookPosterImagefiles { get; set; }
        [NotMapped]
        public IFormFile? BookHoverImageFiles { get; set; }
        [NotMapped]
        public List<IFormFile>? BookImageFiles { get; set; }

        [NotMapped]
        public List<int>? BookImageIds { get; set; }
    }
}
