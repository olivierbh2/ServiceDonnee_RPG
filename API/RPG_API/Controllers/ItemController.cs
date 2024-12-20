﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPG_API.Data.Context;
using RPG_API.Models.Base;
using RPG_API.Models;

namespace RPG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly APIContext _context;

        public ItemController(APIContext context)
        {
            _context = context;
        }

        //GET: api/Item/Get/{id}
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            Item item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        //GET: api/Item/Get/{name}
        [HttpGet("[action]/{name}")]
        public async Task<ActionResult<Item>> GetByName(string name)
        {
            Item item = await _context.Item.FirstOrDefaultAsync(i => i.Name == name);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        //GET: api/Item/Get/{type}
        [HttpGet("[action]/{type}")]
        public async Task<ActionResult<PaginatedList<Item>>> GetByType(int type, int? pageNumber = 1, int pageSize = 10)
        {
            TypeItem typeItem;

            switch (type)
            {
                case 0:
                    typeItem = TypeItem.weapon;
                    break;

                case 1:
                    typeItem = TypeItem.armor;
                    break;

                case 2:
                    typeItem = TypeItem.consumable;
                    break;

                default:
                    return NotFound("Aucun item de ce type n'a été trouvé.");
            }

            // Query to get items of the specified type
            var items = _context.Item.Where(i => i.Type == typeItem).AsQueryable();

            // Get the total count of items for the specified type
            var totalCount = await items.CountAsync();

            // Check if no items were found
            if (totalCount == 0)
            {
                return NotFound("Aucun item de ce type n'a été trouvé.");
            }

            // Apply pagination using PaginatedList
            var pagedItems = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize);

            // Return the paginated list
            return Ok(pagedItems);
        }
        //GET: api/Item/GetAll
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<Item>>> GetAll(int? pageNumber = 1, int pageSize = 10)
        {
            var items = _context.Item.AsQueryable();

            // Utiliser PaginatedList pour créer une liste paginée
            var pagedItems = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize);

            var totalCount = await items.CountAsync();
            // Vérifier si des items ont été trouvés
            if (totalCount == 0)
            {
                return NotFound("Aucun item n'a été trouvé qui correspond à ces critères.");
            }
            return Ok(pagedItems);

        }
        //GET: api/Item/SearchItemsByName
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<Item>>> SearchItemsByName(string? firstLetter, string? nameContains, int? pageNumber = 1, int pageSize = 10)
        {
            var items = _context.Item.AsQueryable();

            // Appliquer le filtre pour le nom qui commence par la première lettre spécifique
            if (!string.IsNullOrEmpty(firstLetter))
            {
                items = items.Where(i => i.Name.StartsWith(firstLetter));
            }

            if (!string.IsNullOrEmpty(nameContains))
            {
                items = items.Where(i => EF.Functions.Like(i.Name, $"%{nameContains}%"));
            }


            // Calculer le nombre total avant la pagination
            var totalCount = await items.CountAsync();

            // Utiliser PaginatedList pour créer une liste paginée
            var pagedItems = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize);

            // Vérifier si des items ont été trouvés
            if (totalCount == 0)
            {
                return NotFound("Aucun item n'a été trouvé qui correspond à ces critères.");
            }
            return Ok(pagedItems);
        }
        //GET: api/Item/SearchItemByStats
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<Item>>> SearchItemByStats(int? minBoostAttack = 0, int? maxBoostAttack = 0,
            int? minBoostDefence = 0, int? maxBoostDefence = 0,
            int? minHealthRestoration = 0, int? maxHealthRestoration = 0,
            int? pageNumber = 1, int pageSize = 10)
        {
            var items = _context.Item.AsQueryable();

            // Appliquer les filtres pour les statistiques
            if (minBoostAttack.HasValue)
            {
                items = items.Where(i => i.BoostAttack >= minBoostAttack.Value);
            }

            if (maxBoostAttack.HasValue)
            {
                items = items.Where(i => i.BoostAttack <= maxBoostAttack.Value);
            }

            if (minBoostDefence.HasValue)
            {
                items = items.Where(i => i.BoostDefence >= minBoostDefence.Value);
            }

            if (maxBoostDefence.HasValue)
            {
                items = items.Where(i => i.BoostDefence <= maxBoostDefence.Value);
            }

            if (minHealthRestoration.HasValue)
            {
                items = items.Where(i => i.HealthRestoration >= minHealthRestoration.Value);
            }

            if (maxHealthRestoration.HasValue)
            {
                items = items.Where(i => i.HealthRestoration <= maxHealthRestoration.Value);
            }

            // Calculer le nombre total avant la pagination
            var totalCount = await items.CountAsync();

            // Utiliser PaginatedList pour créer une liste paginée

            var pagedItems = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize);

            // Vérifier si des items ont été trouvés
            if (totalCount == 0)
            {
                return NotFound("Aucun item n'a été trouvé qui correspond à ces critères.");
            }
            return Ok(pagedItems);
        }

        //PUT: api/Item/Update/{id}
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromQuery] string? name = null, [FromQuery] double? boostDefence = null, [FromQuery] int? boostAttack = null, [FromQuery] int? healthRestoration = null, [FromQuery] TypeItem? type = null)
        {

            Item newItem = await _context.Item.FindAsync(id);
            if (newItem == null)
            {
                return NotFound($"Item with ID {id} not found.");
            }

            if (name != null)
            {
                newItem.Name = name;
            }
            if (boostDefence != null)
            {
                newItem.BoostDefence = (int)boostDefence;
            }
            if (boostAttack != null)
            {
                newItem.BoostAttack = (int)boostAttack;
            }
            if (healthRestoration != null)
            {
                newItem.HealthRestoration = (int)healthRestoration;

            }
            if (type != null)
            {
                newItem.Type = (TypeItem)type;
            }

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(newItem);
        }

        //POST: api/Item/Create
        [HttpPost("[action]")]
        public async Task<ActionResult<Item>> Create([FromBody] Item item)
        {
            _context.Item.Add(item);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok(item);
        }

        //DELETE: api/Item/Delete/{id}
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Item>> Delete(int id)
        {
            Item item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("L'item a été supprimé.");
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
