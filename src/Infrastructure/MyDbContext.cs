using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class MyDbContext : DbContext // might be wrapped in another repo, in case we want to replace it
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the Product-Category relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)                
            .WithMany(c => c.Products)              
            .HasForeignKey(p => p.CategoryId)       
            .OnDelete(DeleteBehavior.Cascade);      

        // Optional: Make the Category navigation property required
        modelBuilder.Entity<Product>()
            .Navigation(p => p.Category)
            .IsRequired(); // Ensures that Category cannot be null in the Product entity
        
        base.OnModelCreating(modelBuilder);

        var electronicsCategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var booksCategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        var now = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc); // Static DateTime for consistency

        var category1 = new Category
        {
            Id = electronicsCategoryId,
            Name = "Electronics",
            CreatedOn = now,
            UpdatedOn = now
        };

        var category2 = new Category
        {
            Id = booksCategoryId,
            Name = "Books",
            CreatedOn = now,
            UpdatedOn = now
        };

        modelBuilder.Entity<Category>().HasData(
            category1,
            category2
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Laptop",
                Description = "High-performance laptop.",
                Price = 1500m,
                CategoryId = electronicsCategoryId,
                CreatedOn = now,
                UpdatedOn = now
            },
            new Product
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Novel",
                Description = "A gripping mystery novel.",
                Price = 20m,
                CategoryId = booksCategoryId,
                CreatedOn = now,
                UpdatedOn = now
            }
        );
    }
}