using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsManagementSystem.Data;
using PostsManagementSystem.Models;

namespace PostsManagementSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PolylinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PolylinesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/<controller>
        [HttpGet]
        [Route("/api/[controller]")]
        public IEnumerable<PolylineDto> Get()
        {
            return _context.Polylines
                .Include(p => p.Color)
                .Select(p => new PolylineDto {
                    Id = p.Id,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Color = p.Color})
                .ToList();
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public PolylineDto Get(int id)
        {
            return _context.Polylines
                .Include(p => p.Color)
                .Select(p => new PolylineDto
                {
                    Id = p.Id,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Color = p.Color
                })
                .FirstOrDefault(p => p.Id == id);
        }
    }
}
