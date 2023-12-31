﻿namespace examprojectprc.ViewModels
{
    public class OrderViewModel
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string? Note { get; set; }
        public List<CheckoutViewModel>? CheckoutViewModels { get; set; }
    }
}
