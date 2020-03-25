using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int ColorId { get; set; }
    }
}
