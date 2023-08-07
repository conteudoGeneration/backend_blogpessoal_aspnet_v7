using blogpessoal.Model;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controller
{
    [Authorize]
    [Route("~/postagens")]
    [ApiController]
    public class PostagemController : ControllerBase
    {
        private readonly IPostagemService _postagemService;
        private readonly IValidator<Postagem> _postagemValidator;

        public PostagemController(
            IPostagemService postagemService,
            IValidator<Postagem> postagemValidator
            )
        {
            _postagemService = postagemService;
            _postagemValidator = postagemValidator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _postagemService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _postagemService.GetById(id);

            if (Resposta is null)
            {
                return NotFound("Postagem não encontrada!");
            }

            return Ok(Resposta);
        }

        [HttpGet("titulo/{titulo}")]
        public async Task<ActionResult> GetByTitulo(string titulo)
        {
            return Ok(await _postagemService.GetByTitulo(titulo));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Postagem postagem)
        {
            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);

            var Resposta = await _postagemService.Create(postagem);

            if (Resposta is null)
                return BadRequest("Tema não encontrado!");

            return CreatedAtAction(nameof(GetById), new { id = postagem.Id }, postagem);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Postagem postagem)
        {
            if (postagem.Id == 0)
                return BadRequest("Id da Postagem é inválido!");

            var validarPostagem = await _postagemValidator.ValidateAsync(postagem);

            if (!validarPostagem.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarPostagem);

            var Resposta = await _postagemService.Update(postagem);

            if (Resposta is null)
                return NotFound("Postagem e/ou Tema não encontrados!");

            return Ok(Resposta);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var BuscaPostagem = await _postagemService.GetById(id);

            if (BuscaPostagem is null)
                return NotFound("Postagem não encontrada!");

            await _postagemService.Delete(BuscaPostagem);

            return NoContent();

        }

    }

}

