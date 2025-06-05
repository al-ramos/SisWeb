using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SisWebCrud.Models
{
	public class Cliente
	{
		[Key]
		public virtual int Id { get; set; }

		[Required(ErrorMessage = "O Nome é obrigatório.")]
		public virtual string Nome { get; set; } = string.Empty;

		[Required(ErrorMessage = "O Tipo é obrigatório.")]
		public virtual TipoCliente Tipo { get; set; }

		[Required(ErrorMessage = "O Documento é obrigatório.")]
		public virtual string Documento { get; set; } = string.Empty;

		public virtual string Endereco { get; set; } = string.Empty;

		[DataType(DataType.Date)]
		public virtual DateTime DataCadastro { get; set; } = DateTime.UtcNow;

		public virtual IList<Telefone> Telefones { get; set; } = new List<Telefone>();

		public virtual bool IsDeleted { get; set; } = false;

		public virtual SexoCliente Sexo { get; set; }
	}
}
