using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;
using PostsManagementSystem.Models.DTOs;

namespace PostsManagementSystem.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(_context.Posts.ToList());
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            Post post = new Post();

            post.Name = collection["Name"];
            post.NameAR = collection["NameAR"];
            post.Content = collection["Content"];
            post.ImageName = Path.GetRandomFileName() + Path.GetExtension(collection.Files[0].FileName);

            using (var localFile = System.IO.File.OpenWrite("wwwroot/images/posts/" + post.ImageName))
            using (var uploadedFile = collection.Files[0].OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            _context.Add(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int id)
        {
            Post post = _context.Posts.Find(id);

            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Post post = _context.Posts.Find(id);

            post.Name = collection["Name"];
            post.NameAR = collection["NameAR"];
            post.Content = collection["Content"];

            if(collection.Files.Count() > 0)
            {
                post.ImageName = Path.GetRandomFileName() + Path.GetExtension(collection.Files[0].FileName);

                using (var localFile = System.IO.File.OpenWrite("wwwroot/images/posts/" + post.ImageName))
                using (var uploadedFile = collection.Files[0].OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }

            _context.Update(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            _context.Posts.Remove(_context.Posts.Find(id));
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("api/[controller]/[action]")]
        [AllowAnonymous]
        public IEnumerable<Post> Get()
        {
            return _context.Posts
                .ToList();
        }
    }
}