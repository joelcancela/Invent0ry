using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Invent0ry.Model
{
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public List<Category> Categories { get; set; }
        public string Location { get; set; }
        public List<string> Loans { get; set; }

        [JsonIgnore]
        public int Available => Quantity - Loans.Count;

        public Item(string name, int quantity) : this()
        {
            Name = name;
            Quantity = quantity;
        }

        public Item()
        {
            Categories = new List<Category>();
            Loans = new List<string>();
        }
    }
}