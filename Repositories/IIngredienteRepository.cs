using api.Models;

namespace api.Repositories
{
	public interface IIngredienteRepository
	{
		Task<IEnumerable<Ingrediente>> GetIngredientesAsync();
		Task<Ingrediente> GetIngredienteByNameAsync(string nome);
		Task AddIngredienteAsync(Ingrediente ingrediente);
		Task UpdateIngredienteAsync(Ingrediente ingrediente);
		Task DeleteIngredienteAsync(string nome);
	}
}
