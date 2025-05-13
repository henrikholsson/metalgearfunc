

using metalgear.Services.Models;

namespace metalgear.Services;
public class ProductService : IProductService
{

    public Task<Product> UpdateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {

        return await Task.FromResult(new List<Product>
        {
            new Product { Id = 1, Name = "Solid Eye™ Multi-Vision Tactical Eyepiece", Description = "Enhance your battlefield awareness with the Solid Eye — a cutting-edge monocular eyepiece equipped with night vision, infrared scanning, motion tracking, and an integrated HUD. With auto-target identification and real-time data streaming, it’s your eye on the enemy.", Price = 34999.99m },
            new Product { Id = 2, Name = "OctoCamo™ Adaptive Stealth Suit", Description = "Disappear in plain sight with the OctoCamo bodysuit. Designed using smart polymer nanofibers, it instantly adapts its appearance to match your surroundings. Lightweight, bullet-resistant, and moisture-wicking, it's the pinnacle of stealthwear technology", Price = 12999.00m },
            new Product { Id = 3, Name = "MK.II Metal Gear Personal Drone", Description = "Deploy your own AI-powered recon unit. The Metal Gear MK.II drone features stealth mobility, a taser probe for non-lethal takedowns, remote hacking tools, and real-time surveillance. Operated via neural-link or manual joystick.", Price = 1250.00m }
        });
    }

    public Task<Product> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product> CreateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }
}