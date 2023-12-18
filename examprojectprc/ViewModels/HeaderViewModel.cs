using examprojectpr.Core.Models;

namespace examprojectprc.ViewModels
{
    public class HeaderViewModel
    {
        public AppUser User { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Setting> Settings { get; set; }
       
    }
}
