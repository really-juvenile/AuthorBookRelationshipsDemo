using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBookRelationshipsDemo.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace AuthorBookRelationshipsDemo.Data
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;
        public static ISession CreateSession()
        {
            if (_sessionFactory == null)
            {
                _sessionFactory = Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012.ConnectionString("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuthorDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;"))
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<AuthorMap>())
                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                    .BuildSessionFactory();
            }
            return _sessionFactory.OpenSession();
        }
    }
}