using BookBazaar.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookBazaar.Data
{
    public class ApplicationDbContext:IdentityDbContext
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
        public DbSet<Product> products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        //seed categroy table
        //insert data in database category table
        protected override void OnModelCreating(ModelBuilder modelBuilder)//seed data to category table
        {
            //add this line as keys of identity tables are mapped in the OnModelCreating
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Sci-fi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Romantic", DisplayOrder = 3 }

                );
            modelBuilder.Entity<Company>().HasData(
              new Company 
              {
                  Id = 1, 
                  Name="Tech Nepal", 
                  StreetName="New Baneshwor", 
                  City="Kathmandu", 
                  State="Bagmati", 
                  PostalCode="123123", 
                  PhoneNumber="9823123132131"
              },
              new Company
              {
                  Id = 2,
                  Name = "Hello Nepal",
                  StreetName = "Lake Side",
                  City = "Pokhara",
                  State = "Gandaki",
                  PostalCode = "34091231",
                  PhoneNumber = "9834212345"
              },
              new Company
              {
                  Id = 3,
                  Name = "Info Bazzar",
                  StreetName = "Putalisadak",
                  City = "Kathmandu",
                  State = "Bagmati",
                  PostalCode = "5617231",
                  PhoneNumber = "9854325678"
              }





             );
            modelBuilder.Entity<Product>().HasData(
              new Product
              {
                  Id = 1,
                  Title = "Fortune of Time",
                  Author = "Billy Spark",
                  Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                  ISBN = "SWD9999001",
                  ListPrice = 99,
                  Price = 90,
                  Price50 = 85,
                  Price100 = 80,
                  CategoryId = 1,
                  ImgUrl =""
              },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImgUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 2,
                    ImgUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 3,
                    ImgUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImgUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 2,
                    ImgUrl = ""
                });
        }

    }
}
