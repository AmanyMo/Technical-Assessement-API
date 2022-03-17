using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Technical_Assessement_API.Models
{
    public class AuthorsBooks
    {
        public int AuthorID { get; set; }
        public int BookID { get; set; }
        public Author Author { get; set; }
        public Book Book { get; set; }
    }
}
