using back.DTOS;
using back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly appDBcontext _appdbcontext;

        public PersonaController(appDBcontext appdbcontext) {
            _appdbcontext = appdbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersons()
        {
            return await _appdbcontext.Persons.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPerson(int id)
        {
            var person = await _appdbcontext.Persons.FindAsync(id);

            if(person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Persona>> PostPerson(Persona person)
        {
            _appdbcontext.Persons.Add(person);
            await _appdbcontext.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.ID }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Persona person)
        {
            if(id != person.ID)
            {
                return BadRequest();
            }

            _appdbcontext.Entry(person).State = EntityState.Modified;

            try
            {
                await _appdbcontext.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(!PersonExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _appdbcontext.Persons.FindAsync(id);
            if(person == null)
            {
                return NotFound();
            }

            _appdbcontext.Persons.Remove(person);
            await _appdbcontext.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExist(int id)
        {
            return _appdbcontext.Persons.Any(e => e.ID == id);
        }
    }
}
