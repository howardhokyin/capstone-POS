using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string item_name { get; set; }
        public double ItemPrice { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public Item(int id, string name, double price, string description, string category)
        {
            Id = id;
            item_name = name;
            ItemPrice = price;
            Description = description;
            Category = category;
        }

        public Item() { }
    }
}

