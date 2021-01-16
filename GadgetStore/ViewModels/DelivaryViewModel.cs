using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.ViewModels
{
    public class DelivaryViewModel
    {
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Hous { get; set; }
        public int? Porch { get; set; }
        public int? Apartment { get; set; }
        public int PayType { get; set; }
    }
}
