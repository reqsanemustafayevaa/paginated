﻿using System.ComponentModel.DataAnnotations;

namespace examprojectprc.Areas.manage.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required]

        [StringLength(maximumLength: 30, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
