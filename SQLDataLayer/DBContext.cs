using Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace SQLDataLayer
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ExampleData> ExampleDatas { get; set; }


        public DbSet<ExampleDataChild> ExampleDataChilds { get; set; }

        public DbSet<ExampleDataSingle> ExampleDataSingles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExampleData>().HasData(new ExampleData { Id = 1, Name = "Test" }, new ExampleData { Id = 2, Name = "Test 2" });
            builder.Entity<ExampleDataChild>().HasData(new ExampleDataChild { Id = 1, Group = "Test", ExampleDataId = 1  }, new ExampleDataChild { Id = 2, Group = "Test 2", ExampleDataId = 2 });
            builder.Entity<ExampleDataSingle>().HasData(new ExampleDataSingle { Id = 1,  Standard = "Test", ExampleDataId = 1 });
            //builder.Entity<SomeClass>()
            //    .HasOne(e => e.SomeField)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
        }
    }
}
