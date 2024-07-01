using System.Text.Json.Serialization;

namespace api.Models
{
	public class Ingrediente
	{
        public int Id { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        [JsonIgnore]
        public ICollection<ReceitaIngrediente> ReceitaIngredientes { get; set; }
	}
}
