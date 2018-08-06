using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invent0ry.Model
{
    class Inventory
    {
        public ObservableCollection<Item> Items { get; set; }

        public Inventory()
        {
            Items = new ObservableCollection<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }
    }
}