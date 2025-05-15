using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using metalgear.Services;

namespace metalgear.Functions
{
    public class Products
    {
        private readonly ILogger<Products> _logger;
        private readonly IProductService _productService;

        public Products(ILogger<Products> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Function(nameof(GetProducts))]
        public async Task<IActionResult> GetProducts([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "products")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var products = await _productService.GetAllProductsAsync();
            return new OkObjectResult(products);
        }
    }
}
