using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;
using PostsManagementSystem.Models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateState([FromBody] UserDto userDto)
        {
            Polyline polyline = _context.Polylines
                .Where(p => p.Latitude == userDto.Latitude && p.Longitude == userDto.Longitude)
                .FirstOrDefault();

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
    }
}
