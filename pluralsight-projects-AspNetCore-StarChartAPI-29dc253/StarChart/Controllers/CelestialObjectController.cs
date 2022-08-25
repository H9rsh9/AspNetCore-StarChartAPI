using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{Id:int}", Name = "GetById")]
       public IActionResult GetById(int Id)
        {
            
            var cobject = _context.CelestialObject.Find(Id);

            if (cobject == null)
            {
                return NotFound();
            }
            else
            {
                cobject.Satellites = _context.CelestialObject.Where(o => o.OrbitalObjectId == Id).ToList();
                return Ok(cobject);
                
            }
        }

        [HttpGet("{Name}")]
        public IActionResult GetByName(string Name)
        {
            var cnames = _context.CelestialObject.Where(o => o.Name == Name);

            if (!cnames.Any())
            {
                return NotFound();
            }
            else
            {
                 foreach(var cname in cnames)
                {
                    cname.Satellites = _context.CelestialObject.Where(o => o.OrbitalObjectId == cname.Id).ToList();
                }
                return Ok(cnames.ToList());
            }
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var cobjects = _context.CelestialObject.ToList();
            foreach(var cobject in cobjects)
            {
                cobject.Satellites = _context.CelestialObject.Where(o => o.OrbitalObjectId == cobject.Id).ToList();
            }
            return Ok(cobjects);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
          

                _context.CelestialObject.Add(celestialObject);
                _context.SaveChanges();

                return CreatedAtRoute("GetById", new { Id = celestialObject.Id }, celestialObject);
            
        }

        [HttpPut("{Id}")]
        public IActionResult Update(int Id, CelestialObject celestial)
        {
            var cobject = _context.CelestialObject.Find(Id);
            if (cobject == null)
            {
                return NotFound();
            }
            else
            {
                cobject.Name = celestial.Name;
                cobject.OrbitalObjectId = celestial.OrbitalObjectId;
                cobject.OrbitalPeriod = celestial.OrbitalPeriod;
                _context.CelestialObject.Update(cobject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpPatch("{Id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var cobject = _context.CelestialObject.Find(id);

            if (cobject != null)
            {
                cobject.Name = name;
                _context.CelestialObject.Update(cobject);
                _context.SaveChanges();
                return NoContent();
            }

            else
            {
                return NotFound();
            }

        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            var cobject = _context.CelestialObject.Where(o => o.Id == id || o.OrbitalObjectId == id);

            if (!cobject.Any())
            {
                return NotFound();
            }

            else
            {
                _context.CelestialObject.RemoveRange(cobject);
                _context.SaveChanges();
                return NoContent();
            }
        }


    }
}
