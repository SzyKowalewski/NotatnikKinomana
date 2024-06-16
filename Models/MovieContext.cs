﻿using Microsoft.Data.Sqlite;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=movies.db");
        }

        public void EnsureSchemaUpdated()
        {
            Database.EnsureCreated();

           
        }
    }
}
