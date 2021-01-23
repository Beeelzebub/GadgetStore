using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GadgetStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [ForeignKey("User")]
        public string SellerId { get; set; }
        public User Seller { get; set; }

        [ForeignKey("User")]
        public string CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Cart { get; set; }

        public Cart GetCartObject()
        {
            return JsonSerializer.Deserialize<Cart>(Cart);
        }
    }
}
