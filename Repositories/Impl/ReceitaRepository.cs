using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Impl
{
	public class ReceitaRepository : IReceitaRepository
	{
		private readonly AppDbContext _context;

		public ReceitaRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Receita>> GetReceitasAsync()
		{
			return await _context.Receitas
				.Include(r => r.ReceitaIngredientes)
				.ThenInclude(ri => ri.Ingrediente)
				.ToListAsync();
		}

		public async Task AddReceitaAsync(Receita receita)
		{
			_context.Receitas.Add(receita);
			await _context.SaveChangesAsync();
		}

		public async Task<Receita> GetReceitaByNameAsync(string nome)
		{
			return await _context.Receitas
				.Include(r => r.ReceitaIngredientes)
				.ThenInclude(ri => ri.Ingrediente)
				.SingleOrDefaultAsync(r => r.Nome == nome);
		}

		public async Task UpdateReceitaAsync(Receita receita)
		{
			_context.Entry(receita).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteReceitaAsync(string nome)
		{
			var receita = await _context.Receitas.SingleOrDefaultAsync(r => r.Nome == nome);
			if (receita == null)
			{
				throw new InvalidOperationException($"Receita '{nome}' não encontrada.");
			}

			_context.Receitas.Remove(receita);
			await _context.SaveChangesAsync();
		}
	}
}
