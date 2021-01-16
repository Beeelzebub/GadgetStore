using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadgetStore.Models
{
    public class Cart
    {
        public List<CartLine> lineCollection { get; set; }
        public Cart()
        {
            lineCollection = new List<CartLine>();
        }
        public IEnumerable<CartLine> Lines() {return lineCollection; }
        public void AddItem(Gadget gadget)
        {
            CartLine line = lineCollection
                .Where(g => g.Gadget.Id == gadget.Id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine { Gadget = new ShortGadget 
                { Id = gadget.Id, Name = gadget.Name, Price = gadget.Price}, Count = 1 });
            }
            else
            {
                line.Count++;
            }
        }
        public void RemoveItem(Gadget gadget)
        {
            CartLine line = lineCollection
                .Where(g => g.Gadget.Id == gadget.Id)
                .FirstOrDefault();
            if (line != null)
            {
                line.Count--;
            }
            if (line.Count <= 0)
            {
                lineCollection.Remove(line);
            }
        }

        public float GetSum()
        {
            return lineCollection.Sum(l => l.Gadget.Price * l.Count);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }

    public class CartLine
    {
        public ShortGadget Gadget { get; set; }
        public int Count { get; set; }
    }

    public class ShortGadget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

    }
}
