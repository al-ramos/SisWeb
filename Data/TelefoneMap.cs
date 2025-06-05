using FluentNHibernate.Mapping;
using SisWebCrud.Models;

public class TelefoneMap : ClassMap<Telefone>
{
	public TelefoneMap()
	{
		Table("Telefones");

		Id(x => x.Id).GeneratedBy.Identity();

		Map(x => x.Numero).Not.Nullable().Length(15);
		Map(x => x.Ativo).Not.Nullable();

		// Correção: Mapeando corretamente a relação com Cliente
		References(x => x.Cliente)
			.Column("ClienteId")  // Certifique-se de que esta coluna só existe uma vez
			.Not.Nullable()
			.Cascade.None();  // Removendo Cascade para evitar conflito de inserção duplicada
	}
}
