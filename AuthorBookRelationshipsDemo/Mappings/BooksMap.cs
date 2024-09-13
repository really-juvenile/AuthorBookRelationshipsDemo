using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBookRelationshipsDemo.Models;
using FluentNHibernate.Mapping;

namespace AuthorBookRelationshipsDemo.Mappings
{
    public class BooksMap : ClassMap<Book>
    {
        public BooksMap()
        {
            Table("Books");
            Id(o => o.Id).GeneratedBy.Identity();
            Map(o => o.Description);
            References(o => o.Author).Column("AuthId").Cascade.None().Nullable();

        }
    }
}