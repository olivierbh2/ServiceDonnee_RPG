﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPG_API.Data.Context;
using RPG_API.Models;
using RPG_API.Models.Base;

namespace RPG_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly APIContext _context;
        //TODO: changer TOUS les messages d'erreur de FR à EN
        public CharacterController(APIContext context)
        {
            _context = context;
        }

        //GET: api/Character/Get/{id}
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Character>> Get(int id)
        {
            var character = await _context.Character.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }
            return Ok(character);
        }
        //GET: api/Character/Get/{name}
        [HttpGet("[action]/{name}")]
        public async Task<ActionResult<Character>> GetByName(string name)
        {
            Character character = await _context.Character.FirstOrDefaultAsync(i => i.Name == name);

            if (character == null)
            {
                return NotFound("Aucun caractère pour ce nom trouvé.");
            }
            return Ok(character);
        }
        //GET: api/Character/Get/{name}
        [HttpGet("[action]/{class}")]
        public async Task<ActionResult<Character>> GetByClass(int classId)
        {
            Character character = await _context.Character.FirstOrDefaultAsync(i => i.ClassId == classId);

            if (character == null)
            {
                return NotFound("Aucun caractère pour cette classe trouvé");
            }
            return Ok(character);
        }
        //GET: api/Character/GetAll
        [HttpGet("[action]")]
        public async Task<ActionResult<List<Character>>> GetAll()
        {
            List<Character> characters = await _context.Character.ToListAsync();

            if (characters == null || characters.FirstOrDefault() == null)
            {
                return NotFound();
            }
            return Ok(characters);
        }
        //PUT: api/Character/Update/{id}
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Character character)

        {

            if (id != character.Id)
            {
                return BadRequest();
            }

            var newCharacter = await _context.Character.FindAsync(id);
            if (newCharacter == null)
            {
                return NotFound();
            }
            newCharacter.Name = character.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpPut("[action]/{id}&{xp}")]
        public async Task<IActionResult> UpdateXP(int id, int xp)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            if (id != character.Id)
            {
                return BadRequest();
            }

            var Character = await _context.Character.FindAsync(id);
            if (Character == null)
            {
                return NotFound();
            }

            Character.Xp = character.Xp;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Xp actualisé.");
        }
        [HttpPut("[action]/{id}&{damage}")]
        public async Task<IActionResult> UpdateDamage(int id, int damage)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            if (id != character.Id)
            {
                return BadRequest();
            }

            var Character = await _context.Character.FindAsync(id);
            if (Character == null)
            {
                return NotFound();
            }

            Character.Damage = character.Damage;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Dommage actualisé.");
        }
        [HttpPut("[action]/{id}&{armor}")]
        public async Task<IActionResult> UpdateArmor(int id, int armor)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            if (id != character.Id)
            {
                return BadRequest();
            }

            var Character = await _context.Character.FindAsync(id);
            if (Character == null)
            {
                return NotFound();
            }

            Character.Armor = character.Armor;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Armure actualisé.");
        }
        [HttpPut("[action]/{id}&{lives}")]
        public async Task<IActionResult> UpdateLives(int id, int lives)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            if (id != character.Id)
            {
                return BadRequest();
            }

            var Character = await _context.Character.FindAsync(id);
            if (Character == null)
            {
                return NotFound();
            }

            Character.Lives = character.Lives;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Vie actualisé.");
        }

        [HttpPut("[action]/{id}&{questid}")]
        public async Task<IActionResult> AddQuest(int id, int questid)
        {
            // find the character
            Character character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            // find quest 
            Quest quest = await _context.Quest.FindAsync(questid);
            if (quest == null) { return NotFound(); }

            // add quest to character's quests
            character.Quests.Add(quest);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Quête ajouté.");
        }

        //PUT :api/Character/UpdateInventaire/{id}&{itemid}
        [HttpPut("[action]/{id}&{itemid}")]
        public async Task<IActionResult> AddItemToInventory(int id, int itemid)
        {

            // find item 
            Item item = await _context.Item.FindAsync(itemid);
            if (item == null)
            {
                return NotFound();
            }

            // find the character
            Character character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }
            // add item to character's inventory
            character.Inventory.Add(item);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok("Item ajouté.");
        }

        // POST: api/Character
        [HttpPost("[action]")]
        public async Task<ActionResult<Character>> Create([FromBody] Character character)
        {
            _context.Character.Add(character);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Les données fournies sont invalides");
            }

            return Ok(character);
        }

        //DELETE: api/Character/id
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound("Caractère pas trouvé.");
            }

            _context.Character.Remove(character);
            await _context.SaveChangesAsync();

            return Ok("Caractère supprimé.");
        }

        //DELETE: api/Character/id/itemid
        [HttpDelete("[action]/{id}&{itemid}")]
        public async Task<IActionResult> DeleteItemFromInventory(int id, int itemid)
        {
            // find item 
            Item item = await _context.Item.FindAsync(itemid);
            if (item == null)
            {
                return NotFound("Item non trouvé.");
            }

            // find the character
            Character character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound("Item non trouvé.");
            }


            // remove item from character's inventory

            var itemToRemove = character.Inventory.FirstOrDefault(i => i.Equals(item));
            if (itemToRemove != null)
            {
                character.Inventory.Remove(itemToRemove);
                if (character.Inventory.FirstOrDefault(i => i.Equals(item)) == null)
                {
                    // if this item cannot be found in the inventory, remove relation
                    item.Characters.Remove(character);
                }
            }
            else
            {
                return NotFound("Item non trouvé.");
            }



            await _context.SaveChangesAsync();

            return Ok("Item supprimé.");
        }
        //DELETE: api/Character/id/questid
        [HttpDelete("[action]/{id}&{questid}")]
        public async Task<IActionResult> DeleteQuest(int id, int questid)
        {
            // find quest 
            Quest quest = await _context.Quest.FindAsync(questid);
            if (quest == null) { return NotFound("Quête pas trouvé."); }

            // find the character
            Character character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound("Quête pas trouvé.");
            }


            // remove quest from character's quests

            var questToRemove = character.Quests.FirstOrDefault(i => i.Equals(quest));
            if (questToRemove != null)
            {
                character.Quests.Remove(questToRemove);
                if (character.Quests.FirstOrDefault(i => i.Equals(quest)) == null)
                {
                    // if this quest cannot be found in the quests, remove relation
                    quest.Characters.Remove(character);
                }
            }
            else
            {
                return NotFound("Quête pas trouvé.");
            }



            await _context.SaveChangesAsync();

            return Ok("Quête supprimé.");
        }
        private bool CharacterExists(int id)
        {
            return _context.Character.Any(e => e.Id == id);
        }
    }
}
