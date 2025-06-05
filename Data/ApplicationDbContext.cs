using Microsoft.EntityFrameworkCore;
using SisWebCrud.Models;
using System;

public class ApplicationDbContext : DbContext
{
	public DbSet<Cliente> Clientes { get; set; }
	public DbSet<Telefone> Telefones { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Definição do relacionamento entre Cliente e Telefones
		modelBuilder.Entity<Cliente>()
			.HasMany(c => c.Telefones)
			.WithOne(t => t.Cliente)
			.HasForeignKey(t => t.ClienteId);

		modelBuilder.Entity<Telefone>()
			.Property(t => t.Numero)
			.IsRequired();

		modelBuilder.Entity<Cliente>()
			.Property(c => c.Tipo)
			.HasConversion<int>();

		// 🚀 Correção: Substituí `DateTime.UtcNow` por uma data fixa
		modelBuilder.Entity<Cliente>().HasData(
			new Cliente
			{
				Id = 1,
				Nome = "Cliente Exemplo",
				Tipo = TipoCliente.PF,
				Sexo = SexoCliente.Masculino,
				Endereco = "Rua Exemplo, 123",
				DataCadastro = new DateTime(2025, 06, 01) // 🔥 Data fixa para evitar erro!
			}
		);
	}

	public void AtualizarTelefones(Cliente cliente)
	{
		if (cliente.Telefones != null)
		{
			foreach (var telefone in cliente.Telefones)
			{
				telefone.Ativo = false;
			}

			var primeiroTelefone = cliente.Telefones.FirstOrDefault();
			if (primeiroTelefone != null)
			{
				primeiroTelefone.Ativo = true;
			}
		}
	}
}
