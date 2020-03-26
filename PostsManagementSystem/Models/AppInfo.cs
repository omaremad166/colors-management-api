using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models
{
    public class AppInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string ContentAR { get; set; }

        public List<SocialMediaAccount> SocialMediaAccounts { get; set; }
    }
}
