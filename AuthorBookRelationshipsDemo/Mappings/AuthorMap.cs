using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBookRelationshipsDemo.Models;
using FluentNHibernate.Mapping;

namespace AuthorBookRelationshipsDemo.Mappings
{
    public class AuthorMap : ClassMap<Author>
    {
        public AuthorMap()
        {
            Table("Authors");
            Id(u => u.Id).GeneratedBy.Identity();
            Map(u => u.Name);
            HasMany(u=>u.Books).Inverse().Cascade.All();    
            HasOne(u => u.AuthorDetails).Cascade.All().PropertyRef(a => a.Author)
                .Constrained(); //Relationship Integrity COnstraint
        }
    }
}