namespace api.Dto
{
	public class ReceitaDtoCompativel
	{
		public string Nome { get; set; }
		public float PercentualCompatibilidade { get; set; }
		public float Preco { get; set; }
		public List<String> IngredientesDaReceita { get; set; }
		public List<String> IngredientesFaltantes { get; set; }
	}
}
