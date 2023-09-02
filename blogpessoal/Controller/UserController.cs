﻿using blogpessoal.Model;
using blogpessoal.Security;
using blogpessoal.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blogpessoal.Controllers
{

    [Route("~/usuarios")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IValidator<User> _userValidator;
        private readonly IAuthService _authService;

        public UserController(
            IUserService userService,
            IValidator<User> userValidator,
            IAuthService authService
            )
        {
            _userService = userService;
            _userValidator = userValidator;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(long id)
        {
            var Resposta = await _userService.GetById(id);

            if (Resposta is null)
            {
                return NotFound("Usuário não encontrado!");
            }

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<ActionResult> Create([FromBody] User usuario)
        {
            var validarUser = await _userValidator.ValidateAsync(usuario);

            if (!validarUser.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);

            var Resposta = await _userService.Create(usuario);

            if (Resposta is null)
                return BadRequest("Usuário já cadastrado!");

            return CreatedAtAction(nameof(GetById), new { id = Resposta.Id }, Resposta);
        }

        [Authorize]
        [HttpPut("atualizar")]
        public async Task<ActionResult> Update([FromBody] User usuario)
        {
            if (usuario.Id == 0)
                return BadRequest("O Id do Usuário é inválido!");

            var validarUser = await _userValidator.ValidateAsync(usuario);

            if (!validarUser.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validarUser);

            var UserUpdate = await _userService.GetByUsuario(usuario.Usuario);

            if ((UserUpdate is not null) && (UserUpdate.Id != usuario.Id))
                return BadRequest("O Usuário (e-mail) já está em uso por outro usuário.");

            var Resposta = await _userService.Update(usuario);

            if (Resposta is null)
                return BadRequest("Usuário não encontrado!");

            return Ok(Resposta);
        }

        [AllowAnonymous]
        [HttpPost("logar")]
        public async Task<IActionResult> Autenticar([FromBody] UserLogin usuarioLogin)
        {
            var Resposta = await _authService.Autenticar(usuarioLogin);

            if (Resposta == null)
            {
                return Unauthorized("Usuário e/ou Senha inválidos!");
            }

            return Ok(Resposta);
        }

    }
}