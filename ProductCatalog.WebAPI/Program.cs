using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.WebAPI.Endpoints;
using ProductCatalog.WebAPI.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddNpgProductRepository();
builder.Services.AddImageStorage();
builder.Services.AddMapper();
builder.Services.AddProductServices();

WebApplication app = builder.Build();

string staticPrefix = "/api/products/";

app.UseCRUDEndpoints(staticPrefix);
app.UseFileEndpoints(staticPrefix);
app.UsePageEndpoints(staticPrefix);

app.Run();
