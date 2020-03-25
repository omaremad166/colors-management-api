using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models
{
    public class Polyline
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }
    }
}
