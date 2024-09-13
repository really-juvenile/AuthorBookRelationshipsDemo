using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;

namespace AuthorBookRelationshipsDemo.Models
{
    public class Author
    {
        public virtual  int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual  string Email { get; set; }
        public virtual  int Age {get; set; }
        public virtual AuthorDetails AuthorDetails { get; set; } = new AuthorDetails();
        public virtual IList<Book> Books { get; set; } = new List<Book>();
    }
}