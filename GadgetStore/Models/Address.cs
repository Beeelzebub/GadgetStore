using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Hous { get; set; }
        public int? Porch { get; set; }
        public int? Apartment { get; set; }

        public override string ToString()
        {
            string addressString = City + ", " + Street + ", " + Hous;
            if (Porch != null)
            {
                addressString += ", " + Porch;
            }
            if (Apartment != null)
            {
                addressString += ", " + Apartment;
            }

            return addressString;
        }

    }
}
