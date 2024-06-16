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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=movies.db");
        }

        public void EnsureSchemaUpdated()
        {
            Database.EnsureCreated();

            using (var connection = (SqliteConnection)Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        PRAGMA foreign_keys=off;

                        CREATE TABLE IF NOT EXISTS Movies_temp1 (
                            Id INTEGER PRIMARY KEY,
                            Title TEXT,
                            Genre TEXT,
                            PremiereDate TEXT,
                            Review TEXT,
                            Rating INTEGER,
                            IsInSchedule INTEGER DEFAULT 0,
                            Description TEXT,
                            Runtime INTEGER DEFAULT 0
                        );

                        INSERT INTO Movies_temp1 (Id, Title, Genre, PremiereDate, Review, Rating, IsInSchedule, Description, Runtime )
                        SELECT Id, Title, Genre, PremiereDate, Review, Rating, IsInSchedule, Description, Runtime
                        FROM Movies;

                        DROP TABLE Movies;

                        ALTER TABLE Movies_temp1 RENAME TO Movies;

                        PRAGMA foreign_keys=on;
                    ";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
