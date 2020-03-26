using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsManagementSystem.Models.ViewModels
{
    public class AboutVM
    {
        public AppInfo AppInfo { get; set; }
        public List<SocialMediaAccount> SocialMediaAccounts { get; set; }
    }
}
