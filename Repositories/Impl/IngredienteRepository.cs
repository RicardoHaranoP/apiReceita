using api.Data;
using api.Exceptions;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Impl
{
	public class IngredienteRepository : IIngredienteRepository
	{
		private readonly AppDbContext _context;

		public IngredienteRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Ingrediente>> GetIngredientesAsync()
		{
			return await _context.Ingredientes.ToListAsync();
		}

		public async Task<Ingrediente> GetIngredienteByNameAsync(string nome)
		{
			return await _context.Ingredientes.SingleOrDefaultAsync(i => i.Nome == nome);
		}

		public async Task AddIngredienteAsync(Ingrediente ingrediente)
		{
			_context.Ingredientes.Add(ingrediente);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateIngredienteAsync(Ingrediente ingrediente)
		{
			_context.Entry(ingrediente).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteIngredienteAsync(string nome)
		{
			var ingrediente = await _context.Ingredientes
				.Include(i => i.ReceitaIngredientes)
				.SingleOrDefaultAsync(i => i.Nome == nome);
			if (ingrediente == null)
			{
				throw new InvalidOperationException($"Ingrediente '{nome}' não encontrado.");
			}
			if (ingrediente.ReceitaIngredientes.Any())
			{
				throw new IngredienteEmUsoException(
					$"Ingrediente '{nome}' está presente em uma receita. Exclua as receitas em que ele está presente antes de excluir o ingrediente.");
			}

			_context.Ingredientes.Remove(ingrediente);
			await _context.SaveChangesAsync();
		}
	}
}
