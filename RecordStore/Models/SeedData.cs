using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace RecordStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider services)
        {
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
            //context.Database.Migrate();
            if (!context.Products.Any())
            {
            context.Products.AddRange
            (
                new Product
                {
                    Name = "Animals",
                    Artist = "Pink Floyd",
                    Genre = "Progressive Rock",
                    Price = 12
                },
                new Product
                {
                    Name = "Acid Rap",
                    Artist = "Chance The Rapper",
                    Genre = "Rap",
                    Price = 2
                },
                new Product
                {
                    Name = "Mantiis",
                    Artist = "Obsidian Kingdom",
                    Genre = "Progressive Black Metal",
                    Price = 25
                },
                new Product
                {
                    Name = "Madvilliany",
                    Artist = "Madvillian",
                    Genre = "Rap",
                    Price = 18
                },
                new Product
                {
                    Name = "Appetite For Destruction",
                    Artist = "Guns N' Roses",
                    Genre = "Rock",
                    Price = 12
                },
                new Product
                {
                    Name = "Currents",
                    Artist = "Tame Impala",
                    Genre = "Psychedelic Rock",
                    Price = 9
                },
                new Product
                {
                    Name = "The Great Misdirect",
                    Artist = "Between The Buried and Me",
                    Genre = "Progressive Metal",
                    Price = 8
                },
                new Product
                {
                    Name = "Songs For The Deaf",
                    Artist = "Queens Of The Stone Age",
                    Genre = "Rock",
                    Price = 14
                },
                        new Product
                        {
                            Name = "In The Aeroplane Over The Sea",
                            Artist = "Neutral Milk Hotel",
                            Genre = "Folk",
                            Price = 26
                        }
                );
                context.SaveChanges();
            }
        }
    }
}
