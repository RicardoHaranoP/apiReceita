using api.Models;

namespace api.Repositories
{
	public interface IReceitaRepository
	{
		Task<IEnumerable<Receita>> GetReceitasAsync();
		Task AddReceitaAsync(Receita receita);
		Task<Receita> GetReceitaByNameAsync(string nome);
		Task UpdateReceitaAsync(Receita receita);
		Task DeleteReceitaAsync(string nome);
	}
}
