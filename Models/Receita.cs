namespace api.Models
{
	public class Receita
	{
		public int Id { get; set; }
		public string Nome { get; set; }
		public ICollection<ReceitaIngrediente> ReceitaIngredientes { get; set; }
	}
}
