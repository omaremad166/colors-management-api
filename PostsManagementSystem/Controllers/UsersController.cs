using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<User> Get()
        {
            return _context.Users
                .Include(u => u.Color)
                .ToList();
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
    }
}
