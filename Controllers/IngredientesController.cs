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
	public class IngredientesController : ControllerBase
	{
		private readonly IIngredienteService _ingredienteService;

		public IngredientesController(IIngredienteService ingredienteService)
		{
			_ingredienteService = ingredienteService;
		}

		[HttpGet("{nome}")]
		public async Task<ActionResult<IngredienteDTO>> GetIngrediente(string nome)
		{
			try
			{
				var ingrediente = await _ingredienteService.GetIngredienteByNameAsync(nome);
				return Ok(ingrediente);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<IngredienteDTO>>> GetIngredientes()
		{
			var ingredientes = await _ingredienteService.GetIngredientesAsync();
			return Ok(ingredientes);
		}

		[HttpPost]
		public async Task<ActionResult> PostIngrediente(IngredienteDTO ingredienteDto)
		{
			if (ingredienteDto.Nome == "" || ingredienteDto.Nome == null)
			{
				return BadRequest("O nome do ingrediente não pode ser vazio");
			}
			if (ingredienteDto.Preco <= 0)
			{
				return BadRequest("O preço deve ser maior que 0");
			}
			try
			{
				await _ingredienteService.AddIngredienteAsync(ingredienteDto);
				return CreatedAtAction(nameof(GetIngredientes), new { nome = ingredienteDto.Nome }, ingredienteDto);
			}
			catch (InvalidOperationException ex)
			{
				return Conflict(ex.Message);
			}
		}

		[HttpPut("{nome}")]
		public async Task<IActionResult> PutIngrediente(string nome, IngredienteDTO ingredienteDto)
		{
			if (ingredienteDto.Nome == "" || ingredienteDto.Nome == null)
			{
				return BadRequest("O nome do ingrediente não pode ser vazio");
			}
			if (ingredienteDto.Preco <= 0)
			{
				return BadRequest("O preço deve ser maior que 0");
			}
			try
			{
				await _ingredienteService.UpdateIngredienteAsync(nome, ingredienteDto);
				return NoContent();
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete("{nome}")]
		public async Task<IActionResult> DeleteIngrediente(string nome)
		{
			try
			{
				await _ingredienteService.DeleteIngredienteAsync(nome);
				return NoContent();
			}
			catch (IngredienteEmUsoException ex)
			{
				return Conflict(ex.Message);
			}
			catch (InvalidOperationException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
