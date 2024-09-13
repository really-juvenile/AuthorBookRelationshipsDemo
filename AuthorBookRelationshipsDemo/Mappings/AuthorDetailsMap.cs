using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBookRelationshipsDemo.Models;
using FluentNHibernate.Mapping;

namespace AuthorBookRelationshipsDemo.Mappings
{
    public class AuthorDetailsMap : ClassMap<AuthorDetails>
    {
        public AuthorDetailsMap()
        {
            Table("AuthorDetails");
            Id(a => a.Id).GeneratedBy.Identity();
            Map(a => a.Street);
            Map(a => a.City);
            
            Map(a => a.Country);
            References(a => a.Author).Column("author_id").Unique().Cascade.None();
        }
    }
}