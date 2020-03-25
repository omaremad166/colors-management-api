using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Content { get; set; }
        public string ImageName { get; set; }
    }
}
