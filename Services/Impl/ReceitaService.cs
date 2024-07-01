using api.Dto;
using api.Exceptions;
using api.Models;
using api.Repositories;
using api.Repositories.Impl;

namespace api.Services.Impl
{
	public class ReceitaService : IReceitaService
    {
        private readonly IReceitaRepository _receitaRepository;
        private readonly IIngredienteRepository _ingredienteRepository;

        public ReceitaService(IReceitaRepository receitaRepository, IIngredienteRepository ingredienteRepository)
        {
            _receitaRepository = receitaRepository;
            _ingredienteRepository = ingredienteRepository;
        }

        public async Task<IEnumerable<ReceitaDtoOutput>> GetReceitasAsync()
        {
            var receitas = await _receitaRepository.GetReceitasAsync();
            return receitas.Select(r => new ReceitaDtoOutput
		    {
			    Nome = r.Nome,
				Preco = r.ReceitaIngredientes.Sum(
					ri => ri.Ingrediente.Preco),
				Ingredientes = r.ReceitaIngredientes
				    .Select(ri => new IngredienteDTO
				    {
					    Nome = ri.Ingrediente.Nome,
					    Preco = ri.Ingrediente.Preco
				    })
				    .ToList()
			});
	    }

        public async Task AddReceitaAsync(ReceitaDtoInput receitaDto)
        {
			var existingReceita = await _receitaRepository.GetReceitaByNameAsync(receitaDto.Nome);
			if (existingReceita != null)
			{
				throw new InvalidOperationException($"Receita '{receitaDto.Nome}' já existe.");
			}

			var receita = new Receita
            {
                Nome = receitaDto.Nome,
                ReceitaIngredientes = receitaDto.Ingredientes
                    .Select(i =>
                    {
                        var ingrediente = _ingredienteRepository.GetIngredienteByNameAsync(i).Result;
                        if (ingrediente == null)
                        {
                            throw new IngredienteNaoExisteException($"Ingrediente '{i}' não encontrado.");
                        }
                        return new ReceitaIngrediente
                        {
                            Ingrediente = ingrediente
                        };
                    })
                    .ToList()
            };



			await _receitaRepository.AddReceitaAsync(receita);
        }

		public async Task<ReceitaDtoOutput> GetReceitaByNameAsync(string nome)
		{
			var receita = await _receitaRepository.GetReceitaByNameAsync(nome);
			if (receita == null)
			{
				throw new InvalidOperationException($"Receita '{nome}' não encontrada.");
			}

			return new ReceitaDtoOutput
			{
				Nome = receita.Nome,
				Ingredientes = receita.ReceitaIngredientes
					.Select(ri => new IngredienteDTO { 
						Nome = ri.Ingrediente.Nome,
						Preco = ri.Ingrediente.Preco
					})
					.ToList()
			};
		}

		public async Task UpdateReceitaAsync(String nome, ReceitaDtoInput receitaDto)
		{
			var receita = await _receitaRepository.GetReceitaByNameAsync(nome);
			if (receita == null)
			{
				throw new InvalidOperationException($"Receita '{nome}' não encontrada.");
			}

			foreach (var ingDto in receitaDto.Ingredientes)
			{
				var ingrediente = await _ingredienteRepository.GetIngredienteByNameAsync(ingDto);
				if (ingrediente == null)
				{
					throw new IngredienteNaoExisteException($"Ingrediente '{ingDto}' não encontrado.");
				}
				receita.ReceitaIngredientes.Add(new ReceitaIngrediente { Ingrediente = ingrediente });
			}

			receita.Nome = receitaDto.Nome;
			receita.ReceitaIngredientes = receitaDto.Ingredientes
				.Select(i =>
				{
					var ingrediente = _ingredienteRepository.GetIngredienteByNameAsync(i).Result;
					if (ingrediente == null) throw new IngredienteNaoExisteException($"Ingrediente '{i}' não encontrado.");
					return new ReceitaIngrediente { Ingrediente = ingrediente };
				})
				.ToList();			

			await _receitaRepository.UpdateReceitaAsync(receita);
		}

		public async Task DeleteReceitaAsync(string nome)
		{
			await _receitaRepository.DeleteReceitaAsync(nome);
		}

		public async Task<IEnumerable<ReceitaDtoCompativel>> GetReceitasCompatibilidadeAsync(List<string> ingredientes)
		{
			var receitas = await _receitaRepository.GetReceitasAsync();
			var receitasCompatibilidade = new List<ReceitaDtoCompativel>();

			foreach (var receita in receitas)
			{
				var receitaDTO = new ReceitaDtoCompativel
				{
					Nome = receita.Nome,
					Preco = receita.ReceitaIngredientes.Sum(
						ri => ri.Ingrediente.Preco),
					IngredientesDaReceita = new List<String>(),
					IngredientesFaltantes = new List<String>()
				};

				int totalIngredientes = receita.ReceitaIngredientes.Count;
				int matchCount = 0;

				foreach (var receitaIngrediente in receita.ReceitaIngredientes)
				{
					bool isMatch = ingredientes.Contains(receitaIngrediente.Ingrediente.Nome);
					if (isMatch) matchCount++;
					else receitaDTO.IngredientesFaltantes.Add(receitaIngrediente.Ingrediente.Nome);

					receitaDTO.IngredientesDaReceita.Add(receitaIngrediente.Ingrediente.Nome);
				}

				receitaDTO.PercentualCompatibilidade = (float)matchCount / totalIngredientes * 100;
				receitasCompatibilidade.Add(receitaDTO);
			}

			return receitasCompatibilidade.OrderByDescending(r => r.PercentualCompatibilidade);
		}
	}	
}
