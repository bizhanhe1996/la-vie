using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LaVie.Models;

namespace LaVie.Data;

public class MyAppContext(DbContextOptions<MyAppContext> options)
    : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public override DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { set; get; }
    public DbSet<Product> Products { set; get; }
    public DbSet<Tag> Tags { set; get; }
    public DbSet<ProductTag> ProductTags { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // unique indexes
        modelBuilder.Entity<Category>().HasIndex(c => c.Title).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(c => c.Slug).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

        // relationships
        OneToManyRelationship(modelBuilder);
        ManyToManyRelationship(modelBuilder);
    }

    private static void OneToManyRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId);
    }

    private static void ManyToManyRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductTag>().HasKey(pt => new { pt.ProductId, pt.TagId });
        modelBuilder
            .Entity<ProductTag>()
            .HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId);
        modelBuilder
            .Entity<ProductTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagId);
    }
}
