using System.ComponentModel.DataAnnotations;

namespace examprojectprc.ViewModels
{
    public class MemberLoginViewModel
    {

        [Required]
        public string Username { get; set; }
        [Required]

        [StringLength(maximumLength: 30, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
