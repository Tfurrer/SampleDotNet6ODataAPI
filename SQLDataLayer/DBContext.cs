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
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<SomeClass>()
            //    .HasOne(e => e.SomeField)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(builder);
        }
    }
}
