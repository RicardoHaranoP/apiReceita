using api.Dto;

namespace api.Services
{
	public interface IIngredienteService
	{
		Task<IEnumerable<IngredienteDTO>> GetIngredientesAsync();
		Task AddIngredienteAsync(IngredienteDTO ingredienteDto);
		Task<IngredienteDTO> GetIngredienteByNameAsync(string nome);
		Task UpdateIngredienteAsync(string nome, IngredienteDTO ingredienteDto);
		Task DeleteIngredienteAsync(string nome);
	}
}
