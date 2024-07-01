namespace api.Dto
{
	public class ReceitaDtoOutput
	{
		public string Nome { get; set; }

		public float Preco { get; set; }
		public List<IngredienteDTO> Ingredientes { get; set; }
	}
}
