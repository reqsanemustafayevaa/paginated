using examprojectpr.Core.Models;

namespace examprojectprc.ViewModels
{
    public class ProductDetailViewModel
    {
        public Book Book { get; set; }
        public List<Book> RelatedBooks { get; set; }
    }
}
