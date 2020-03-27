using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;
using PostsManagementSystem.Models.DTOs;
using PostsManagementSystem.Services;

namespace PostsManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("/api/[controller]")]
        public IEnumerable<User> Get()
        {
            return _context.Users
                .Include(u => u.Color)
                .ToList();
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public User Get(int id)
        {
            return _context.Users
                .Include(u => u.Color)
                .FirstOrDefault(u => u.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateState([FromBody] UserDto userDto)
        {
            Polyline polyline = await _context.Polylines
                .Where(p => p.Latitude == userDto.Latitude && p.Longitude == userDto.Longitude)
                .FirstOrDefaultAsync();

            if (polyline == null) //Empty Point
            {
                if (userDto.ColorId == 1 || userDto.ColorId == 2 || userDto.ColorId == 3) //Red/Orange/Yellow User
                {
                    Color color = _context.Colors.Find(userDto.ColorId);

                    Polyline newPolyline = new Polyline
                    {
                        Latitude = userDto.Latitude,
                        Longitude = userDto.Longitude,
                        Color = color
                    };

                    _context.Add(newPolyline);
                    _context.SaveChanges();

                    return Ok();
                }
            }
            else if (userDto.ColorId == 1) //Red User
            {
                polyline.ColorId = 1;

                _context.Update(polyline);
                _context.SaveChanges();

                return Ok();
            }
            else if (userDto.ColorId == 2) //Orange User
            {
                if (polyline.ColorId == 1)//Red Point
                {
                    User user = _context.Users.Find(userDto.UserId);

                    user.ColorId = polyline.ColorId;

                    _context.Update(user);
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    polyline.ColorId = userDto.ColorId;

                    _context.Update(polyline);
                    _context.SaveChanges();

                    return Ok();
                }
            }
            else if (userDto.ColorId == 3) //Yellow User
            {
                if (polyline.ColorId == 1 || polyline.ColorId == 2) //Red/Orange Point
                {
                    User user = _context.Users.Find(userDto.UserId);

                    user.ColorId = polyline.ColorId;

                    _context.Update(user);
                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    polyline.ColorId = userDto.ColorId;

                    _context.Update(polyline);
                    _context.SaveChanges();

                    return Ok();
                }
            }
            else
            {
                User user = _context.Users.Find(userDto.UserId);

                user.ColorId = polyline.ColorId;

                _context.Update(user);
                _context.SaveChanges();

                return Ok();
            }
            return StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_context.Users.Where(u => u.Email == user.Email).SingleOrDefault() != null)
                return BadRequest(new { message = "User registered with the same E-mail before!" });

            UserService userService = new UserService();

            user.Password = userService.GetHashString(user.Password);

            _context.Add(user);
            _context.SaveChanges();

            return Ok();
        }
        
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            UserService userService = new UserService();

            string hashedPassword = userService.GetHashString(user.Password);

            User authUser = _context.Users
                .Where(u => u.Email == user.Email && u.Password == hashedPassword)
                .FirstOrDefault(); 

            if(authUser != null)
            {
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                return Ok(new
                {
                    Id = authUser.Id,
                    FirstName = authUser.FirstName,
                    LastName = authUser.LastName,
                    Email = authUser.Email,
                    Token = token
                });
            }
            else
                return BadRequest(new { message = "Incorrect E-mail or password!" });
        }

        //Users CRUD
        [Route("/[controller]")]
        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Color).ToList();

            return View(users);
        }

        [Route("/[controller]/[Action]")]
        public IActionResult CreateUser()
        {
            ViewBag.Colors = _context.Colors
                .Select(c => new SelectListItem()
                {
                    Text = c.ColorName,
                    Value = c.Id.ToString()
                });

            return View();
        }

        [Route("/[controller]/[Action]")]
        [HttpPost]
        public IActionResult CreateUser(User user, IFormCollection collection)
        {
            user.ImageName = Path.GetRandomFileName() + Path.GetExtension(collection.Files[0].FileName);

            using (var localFile = System.IO.File.OpenWrite("wwwroot/images/users/" + user.ImageName))
            using (var uploadedFile = collection.Files[0].OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            UserService userService = new UserService();

            user.Password = userService.GetHashString(user.Password);

            _context.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [Route("/[controller]/[Action]")]
        public IActionResult EditUser(int id)
        {
            User user = _context.Users.Find(id);

            ViewBag.Colors = _context.Colors
                .Select(c => new SelectListItem()
                {
                    Text = c.ColorName,
                    Value = c.Id.ToString()
                });

            return View(user);
        }

        [Route("/[controller]/[Action]")]
        [HttpPost]
        public IActionResult EditUser(int id, IFormCollection collection)
        {
            var user = _context.Users.Find(id);

            user.FirstName = collection["FirstName"];
            user.LastName = collection["LastName"];
            user.Email = collection["Email"];
            user.Gender = bool.Parse(collection["Gender"]);
            user.Address = collection["Address"];
            user.WorkAddress = collection["WorkAddress"];
            user.ColorId = Int32.Parse(collection["ColorId"]);

            if (collection.Files.Count() > 0)
            {
                user.ImageName = Path.GetRandomFileName() + Path.GetExtension(collection.Files[0].FileName);

                using (var localFile = System.IO.File.OpenWrite("wwwroot/images/users/" + user.ImageName))
                using (var uploadedFile = collection.Files[0].OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }


            if (user.Password != null)
            {
                UserService userService = new UserService();

                user.Password = userService.GetHashString(user.Password);
            }

            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }

        [Route("/[controller]/[Action]")]
        public IActionResult DeleteUser(int id)
        {
            _context.Users.Remove(_context.Users.Find(id));
            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }
    }
}
