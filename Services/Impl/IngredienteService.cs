using api.Dto;
using api.Models;
using api.Repositories;

namespace api.Services.Impl
{
	public class IngredienteService : IIngredienteService
	{
		private readonly IIngredienteRepository _ingredienteRepository;

		public IngredienteService(IIngredienteRepository ingredienteRepository)
		{
			_ingredienteRepository = ingredienteRepository;
		}

		public async Task<IEnumerable<IngredienteDTO>> GetIngredientesAsync()
		{
			var ingredientes = await _ingredienteRepository.GetIngredientesAsync();
			return ingredientes.Select(i => new IngredienteDTO { Nome = i.Nome, Preco = i.Preco });
		}

		public async Task AddIngredienteAsync(IngredienteDTO ingredienteDto)
		{
			var existingIngrediente = await _ingredienteRepository.GetIngredienteByNameAsync(ingredienteDto.Nome);
			if (existingIngrediente != null)
			{
				throw new InvalidOperationException($"Ingrediente '{ingredienteDto.Nome}' já existe.");
			}

			var ingrediente = new Ingrediente
			{
				Nome = ingredienteDto.Nome,
				Preco = ingredienteDto.Preco
			};
			await _ingredienteRepository.AddIngredienteAsync(ingrediente);
		}

		public async Task<IngredienteDTO> GetIngredienteByNameAsync(string nome)
		{
			var ingrediente = await _ingredienteRepository.GetIngredienteByNameAsync(nome);
			if (ingrediente == null)
			{
				throw new InvalidOperationException($"Ingrediente '{nome}' não encontrado.");
			}

			return new IngredienteDTO { Nome = ingrediente.Nome, Preco = ingrediente.Preco };
		}

		public async Task UpdateIngredienteAsync(String nome, IngredienteDTO ingredienteDto)
		{
			var ingrediente = await _ingredienteRepository.GetIngredienteByNameAsync(nome);
			if (ingrediente == null)
			{
				throw new InvalidOperationException($"Ingrediente '{nome}' não encontrado.");
			}

			ingrediente.Preco = ingredienteDto.Preco;
			ingrediente.Nome = ingredienteDto.Nome;
			await _ingredienteRepository.UpdateIngredienteAsync(ingrediente);
		}

		public async Task DeleteIngredienteAsync(string nome)
		{
			await _ingredienteRepository.DeleteIngredienteAsync(nome);
		}
	}
}
