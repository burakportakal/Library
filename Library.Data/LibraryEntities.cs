using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Library.Data
{
    public class LibraryEntities : IdentityDbContext<ApplicationUser>
    {
        public LibraryEntities() : base("LibraryEntities") { }

        public DbSet<Authors> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Reserve> Reserve { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<BookIds> BookIds { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>().HasKey(e => e.Isbn);
            modelBuilder.Entity<BookIds>().HasKey(e => e.BookId);
            base.OnModelCreating(modelBuilder);
        }
        public static LibraryEntities Create()
        {
            return new LibraryEntities();
        }
    }
}
