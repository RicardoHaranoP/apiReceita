using api.Dto;

namespace api.Services
{
	public interface IReceitaService
	{
		Task<IEnumerable<ReceitaDtoOutput>> GetReceitasAsync();
		Task AddReceitaAsync(ReceitaDtoInput receitaDto);
		Task<ReceitaDtoOutput> GetReceitaByNameAsync(string nome);
		Task UpdateReceitaAsync(string nome, ReceitaDtoInput receitaDto);
		Task DeleteReceitaAsync(string nome);
		Task<IEnumerable<ReceitaDtoCompativel>> GetReceitasCompatibilidadeAsync(List<string> ingredientes);
	}
}
