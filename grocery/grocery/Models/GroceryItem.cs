using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grocery.Models
{
    public class GroceryItem
    {
        public int Id { get; set; }
        public string GroceryName { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        //public System.Byte[] Img { get; set; }
    }

    public class Response
    {
        public string Message { get; set; }
        public List<GroceryItem> GroceryItems { get; set; }
    }
}
