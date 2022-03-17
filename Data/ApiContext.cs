using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Technical_Assessement_API.Models;

namespace Technical_Assessement_API.Data
{
    public class ApiContext :DbContext
    {
        public ApiContext(DbContextOptions<ApiContext>options):base(options)

        {

        }

        public DbSet<Author>Author { get; set; }
        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<AuthorsBooks>().HasKey(compkey => new { compkey.AuthorID, compkey.BookID });

            modelbuilder.Entity<AuthorsBooks>().HasOne(p => p.Author)
                .WithMany(p => p.AuthorsBooks).HasForeignKey(p => p.AuthorID);

            modelbuilder.Entity<AuthorsBooks>().HasOne(p => p.Book)
                .WithMany(prop => prop.AuthorsBooks).HasForeignKey(p=>p.BookID);
        }


    }
}
