using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xablau.Data;
using Xablau.Models;

namespace Xablau.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonagemController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PersonagemController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task <IActionResult> AddPersonagem([FromBody]Personagem personagem)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _appDbContext.XablauDB.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            return Created("Personagem criado com sucesso :) :( ", personagem);
        }

        [HttpGet]
        public async Task<ActionResult <IEnumerable<Personagem>>> GetPersonagem() {

            var personagem = await _appDbContext.XablauDB.ToListAsync();

            return Ok(personagem);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Personagem>> GetPersonagem(int id) {

            var personagem = await _appDbContext.XablauDB.FindAsync(id);

            if(personagem == null) {
                return NotFound("Personagem não encontrado");
            }

            return Ok(personagem);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdatePersonagem(int id, [FromBody] Personagem personagemAtualizado) {

            var personagemExistente = await _appDbContext.XablauDB.FindAsync(id);

            if(personagemExistente == null) {
                return NotFound("Personagem não encontrado");
            }

            _appDbContext.Entry(personagemExistente).CurrentValues.SetValues(personagemAtualizado);

            await _appDbContext.SaveChangesAsync();

            return Ok(personagemExistente);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Personagem>> DeletePersonagem(int id) {

            var personagem = await _appDbContext.XablauDB.FindAsync(id);

            if(personagem == null) {
                return NotFound("Personagem não encontrado");
            }

            _appDbContext.Remove(personagem);

            await _appDbContext.SaveChangesAsync();

            return Ok("Personagem foi de base");
        }
    }
}