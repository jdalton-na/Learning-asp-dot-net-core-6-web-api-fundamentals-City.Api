
using Microsoft.EntityFrameworkCore;
using CityInfo.Api.Entities;

namespace CityInfo.Api.DbContexts
{

    

    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) 
            : base(options) 
        {
        }

        public DbSet<Entities.City> Cities { get; set; } = null!;

        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.City>()
                .HasData(
                    new Entities.City("New York City")
                    {
                        Id = 1,
                        Description = "Big Apple"
                    },
                    new Entities.City("Atlanta")
                    {
                        Id = 2,
                        Description = "The Big Peach"
                    },
                    new Entities.City("Charlotte")
                    {
                        Id = 3,
                        Description = "Queen City"
                    }
                );

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                    new PointOfInterest("Central Park")
                    {
                        Id = 1,
                        CityId = 1,
                        Description = "The most visited park in the United States."
                    },
                    new PointOfInterest("Empire State Building")
                    {
                        Id = 2,
                        CityId = 1,
                        Description = "A 102-story skyscrapper located in Midtown Manhattan."
                    },
                    new PointOfInterest("Braves Stadium")
                    {
                        Id = 3,
                        CityId = 2,
                        Description = "Home of Atlanta Braves Baseball Team."
                    },
                    new PointOfInterest("Olympic Park")
                    {
                        Id = 4,
                        CityId = 2,
                        Description = "Place Eric Rudolph set off bomb during 1996 Olympics."
                    },
                    new PointOfInterest("Charlotte Moter Speedway")
                    {
                        Id = 5,
                        CityId = 3,
                        Description = "Race track home to CoCo-Cola 600 race."
                    }
                );
            base.OnModelCreating(modelBuilder);
        }
    }

}
