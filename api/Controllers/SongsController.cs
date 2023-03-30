using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.DataHandling;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        // GET: api/Songs
        [HttpGet]
        public List<Song> Get()
        {
           var db = new Database();
           return db.GetSongs();
        }

        // GET: api/Songs/5
        [HttpGet("{id}", Name = "Get")]
        public Song Get(int id)
        {
            Database db = new Database();
            return db.GetSong(id);
        }

        // POST: api/Songs
        [HttpPost]
        public Song Post([FromBody] Song song)
        {
            Database db = new Database();
            var newSong = db.AddSong(song);
            return newSong;
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Song song)
        {
            Database db = new Database();
            db.UpdateSong(id, song);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Database db = new Database();
            db.SoftDelete(id);
        }
    }
}
