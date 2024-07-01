using System.Text.Json.Serialization;

namespace api.Models
{
	public class ReceitaIngrediente
	{
		public int ReceitaId { get; set; }
		[JsonIgnore]
		public Receita Receita { get; set; }

		public int IngredienteId { get; set; }
		[JsonIgnore]
		public Ingrediente Ingrediente { get; set; }
	}
}
