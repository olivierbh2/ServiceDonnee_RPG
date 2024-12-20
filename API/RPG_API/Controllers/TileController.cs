﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPG_API.Data.Context;
using RPG_API.Models;
using RPG_API.Models.Base;

namespace RPG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TileController : Controller
    {
        private readonly APIContext _context;

        public TileController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Tile/Get/{id}
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Tile>> Get(int id)
        {
            Tile tile = await _context.Tile.FindAsync(id);

            if (tile == null)
            {
                return NotFound();
            }
            return Ok(tile);
        }

        // GET: api/Tile/GetAll
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<Tile>>> GetAll(int? pageNumber = 1, int pageSize = 10)
        {
            var tiles = _context.Tile.AsQueryable();

            var totalCount = await tiles.CountAsync();

            if (totalCount == 0)
            {
                return NotFound("Aucune tuile de ce type n'a été trouvé.");
            }

            var pagetTiles = await PaginatedList<Tile>.CreateAsync(tiles.AsNoTracking(), pageNumber ?? 1, pageSize);

            return Ok(pagetTiles);
        }

        // PUT: api/Tile/Update/{id}/{newX}/{newY}/{newType}
        [HttpPut("[action]/{id}")]
        public async Task<ActionResult<Tile>> Update(int id, [FromQuery] int? newX = null, [FromQuery] int? newY = null, [FromQuery] int? newType = null, [FromQuery] int? newMapId = null)
        {
            var tile = await _context.Tile.FindAsync(id);
            if (tile == null)
            {
                return NotFound();
            }

            // Update only the specified properties
            if (newX != null)
            { tile.X = (int) newX;}

            if (newY != null)
            { tile.Y = (int) newY; }

            if (newMapId != null)
            { tile.MapId = (int) newMapId; }

            if (newType.HasValue)
            {
                if (Enum.IsDefined(typeof(TypeTile), newType))
                { tile.Type = (TypeTile)newType; }

                else
                {
                    return BadRequest("Type invalide");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(tile);
        }

        // POST: api/Tile/Create
        [HttpPost("[action]")]
        public async Task<ActionResult<Tile>> Create([FromBody] Tile tile)
        {
            _context.Tile.Add(tile);
            await _context.SaveChangesAsync();

            return Ok(tile);
        }

        // DELETE: api/Tile/Delete/{id}
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Tile>> Delete(int id)
        {
            Tile tile = await _context.Tile.FindAsync(id);
            if (tile == null)
            {
                return NotFound();
            }

            _context.Tile.Remove(tile);
            await _context.SaveChangesAsync();

            return Ok(tile);
        }
    }
}
