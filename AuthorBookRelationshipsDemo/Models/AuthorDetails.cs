using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorBookRelationshipsDemo.Models
{
    public class AuthorDetails
    {
        public virtual int Id { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        
        public virtual string Country { get; set; }

        public virtual Author Author { get; set; }
    }
}