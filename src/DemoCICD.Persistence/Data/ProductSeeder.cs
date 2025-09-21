using DemoCICD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoCICD.Persistence.Data;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return; // Data already exists

        var products = new List<Product>
        {
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-MotoGP Helmet Pro",
                599.99m,
                "Professional racing helmet with advanced safety features and aerodynamic design"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Racing Gloves Elite",
                149.99m,
                "Premium racing gloves with enhanced grip and protection for professional riders"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Leather Jacket Premium",
                899.99m,
                "High-quality leather racing jacket with CE-approved armor and thermal regulation"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Racing Boots Pro",
                499.99m,
                "Professional racing boots with ankle protection and superior grip technology"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Back Protector Carbon",
                299.99m,
                "Lightweight carbon fiber back protector for maximum protection and comfort"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Knee Sliders Competition",
                79.99m,
                "Durable knee sliders designed for competitive racing with superior abrasion resistance"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Racing Suit Complete",
                1299.99m,
                "One-piece leather racing suit with integrated protection and aerodynamic design"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Communication System Pro",
                399.99m,
                "Advanced Bluetooth communication system for helmet integration with noise cancellation"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Tool Kit Essential",
                199.99m,
                "Complete motorcycle maintenance tool kit with all essential tools for racing bikes"
            ),
            Product.CreateProduct(
                Guid.NewGuid(),
                "ABCD-Tire Pressure Monitor",
                249.99m,
                "Digital tire pressure monitoring system with real-time data transmission"
            )
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}