using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotatnikKinomana.Models
{
    internal class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Watched> Watched { get; set; }
        public DbSet<Director> Directors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=movies.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany()
                .HasForeignKey(m => m.DirectorId);
        }

        public void EnsureSchemaUpdated()
        {
            Database.EnsureCreated();

           
        }
    }
}
