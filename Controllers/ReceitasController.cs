namespace api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using api.Data;
    using api.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using api.Dto;
    using api.Services;
    using api.Exceptions;

    [ApiController]
    [Route("api/[controller]")]
    public class ReceitasController : ControllerBase
    {
        private readonly IReceitaService _receitaService;

        public ReceitasController(IReceitaService receitaService)
        {
            _receitaService = receitaService;
        }

        [HttpGet("{nome}")]
        public async Task<ActionResult<ReceitaDtoOutput>> GetReceita(string nome)
        {
            try
            {
                var receita = await _receitaService.GetReceitaByNameAsync(nome);
                return Ok(receita);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceitaDtoOutput>>> GetReceitas()
        {
            var receitas = await _receitaService.GetReceitasAsync();
            return Ok(receitas);
        }

        [HttpPost]
        public async Task<ActionResult> PostReceita(ReceitaDtoInput receitaDto)
        {
            if (receitaDto.Nome == "" || receitaDto.Nome == null)
            {
                return BadRequest("O nome da receita não pode ser vazio");
            }
            if (receitaDto.Ingredientes.Count == 0)
            {
                return BadRequest("A lista de ingredientes não pode ser vazia");
            }
            try
            {
                await _receitaService.AddReceitaAsync(receitaDto);
                return CreatedAtAction(nameof(GetReceita), new { nome = receitaDto.Nome }, receitaDto);
            }
            catch (IngredienteNaoExisteException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{nome}")]
        public async Task<IActionResult> PutReceita(string nome, ReceitaDtoInput receitaDto)
        {
            if (receitaDto.Nome == "" || receitaDto.Nome == null)
            {
                return BadRequest("O nome da receita não pode ser vazio");
            }
            if (receitaDto.Ingredientes.Count == 0)
            {
                return BadRequest("A lista de ingredientes não pode ser vazia");
            }
            try
            {
                await _receitaService.UpdateReceitaAsync(nome, receitaDto);
                return NoContent();
            }
            catch (IngredienteNaoExisteException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{nome}")]
        public async Task<IActionResult> DeleteReceita(string nome)
        {
            try
            {
                await _receitaService.DeleteReceitaAsync(nome);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("compatibilidade")]
        public async Task<ActionResult<IEnumerable<ReceitaDtoCompativel>>> GetReceitasCompatibilidade(List<string> ingredientes)
        {
            if (ingredientes == null || !ingredientes.Any())
            {
                return BadRequest("A lista de ingredientes não pode ser vazia.");
            }

            var receitasCompatibilidade = await _receitaService.GetReceitasCompatibilidadeAsync(ingredientes);
            return Ok(receitasCompatibilidade);
        }

    }
}
