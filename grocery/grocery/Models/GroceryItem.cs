using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grocery.Models
{
    public class GroceryItem
    {
        public string GroceryName { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Img { get; set; }
    }
}
