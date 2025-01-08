using BookBazaar.Model;
using Microsoft.EntityFrameworkCore;

namespace BookBazaar.Data
{
    public class ApplicationDbContext:DbContext
    {
        //implementation of dbcontext
        //dbcontext is the root class of ef core which facilities interaction with the db.
        //it provides api for quering data and saving it
        //it maps your c# model with db table
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            //to configure dbcontext with setting like connection string and
            //pass it to base class which is DbContext
            
        }

        public DbSet<Category> categories { get; set; }//model's connection with db table
                                                       //to create categories table in database.

        //seed categroy table
        //insert data in database category table
        protected override void OnModelCreating(ModelBuilder modelBuilder)//seed data to category table
        {
            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci-fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Romantic", DisplayOrder = 3 }

                );
        }


    }
}
