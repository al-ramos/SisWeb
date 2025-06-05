using FluentNHibernate.Mapping;
using SisWebCrud.Models;

public class ClienteMap : ClassMap<Cliente>
{
	public ClienteMap()
	{
		Table("Clientes");

		Id(x => x.Id).GeneratedBy.Identity();

		Map(x => x.Nome).Not.Nullable().Length(200);
		Map(x => x.Tipo)
			.CustomType<int>()  // Aqui garantimos que será tratado como int
			.Column("Tipo")     // Certificando que o nome do campo no banco é "Tipo"
			.Not.Nullable();
		Map(x => x.Documento).Not.Nullable().Length(50);
		Map(x => x.Endereco).Nullable().Length(500);
		Map(x => x.DataCadastro).Not.Nullable();
		Map(x => x.IsDeleted).Not.Nullable();
		Map(x => x.Sexo) // 🔥 Confirme que isso está presente
			.CustomType<int>()
			.Column("Sexo")
			.Not.Nullable();

		HasMany(x => x.Telefones)
			.KeyColumn("ClienteId")
			.Inverse()
			.Cascade.AllDeleteOrphan();
	}
}
