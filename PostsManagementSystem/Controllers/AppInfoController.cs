using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PostsManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AppInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("/api/[controller]")]
        public IEnumerable<AppInfo> Get()
        {
            return _context.AppInfo.ToList();
        }
    }
}
