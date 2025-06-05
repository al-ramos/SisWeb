using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Reflection;

public class NHibernateHelper
{
    private static NHibernate.ISessionFactory _sessionFactory;

    public static NHibernate.ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
                _sessionFactory = CreateSessionFactory();

            return _sessionFactory;
        }
    }

    private static NHibernate.ISessionFactory CreateSessionFactory()
    {
        return Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString("Server=.;Database=SisWebCrud;Trusted_Connection=True;TrustServerCertificate=True;")
                .ShowSql() // opcional: exibe as queries geradas no console
            )
            .Mappings(m => m.FluentMappings
                .AddFromAssembly(Assembly.GetExecutingAssembly()))
            .BuildSessionFactory();
    }

    public static NHibernate.ISession OpenSession()
    {
        return SessionFactory.OpenSession();
    }
}
