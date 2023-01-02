using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grocery.Models
{
    public class AdminSignUp
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Created_by { get; set; }
        public string IsActive { get; set; }
    }
}
