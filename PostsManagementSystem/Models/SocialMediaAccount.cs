using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models
{
    public class SocialMediaAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }

        public int AppInfoId { get; set; }
        public AppInfo AppInfo { get; set; }
    }
}
