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
    [Route("api/[controller]")]
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<ContactUs> Get()
        {
            return _context.ContactUs.ToList();
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]ContactUs info)
        {
            _context.ContactUs.Add(info);
            _context.SaveChanges();
        }

        [Route("/ContactUsMsgs")]
        public ActionResult Index()
        {
            return View("../Home/ContactUs", _context.ContactUs.ToList());
        }
    }
}
