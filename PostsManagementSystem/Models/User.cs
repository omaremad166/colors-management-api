using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
