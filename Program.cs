using metalgear.Middleware;
using metalgear.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Configure middleware
builder.UseMiddleware<DemoMiddleware>();

builder.Services.AddSingleton<IProductService, ProductService>();

builder.Build().Run();
