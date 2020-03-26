using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAR { get; set; }
        public string Content { get; set; }
        //public File Image();
        public Stream FileStream { get; set; }
    }
}
