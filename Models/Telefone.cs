using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SisWebCrud.Models
{
	public class Telefone
	{
		[Key]
		public virtual int Id { get; set; }

		[Required(ErrorMessage = "O Número é obrigatório.")]
		public virtual string Numero { get; set; } = string.Empty;

		public virtual bool Ativo { get; set; }

		public virtual int ClienteId { get; set; }

		[ForeignKey("ClienteId")]
		public virtual Cliente Cliente { get; set; } = new Cliente();
	}
}
