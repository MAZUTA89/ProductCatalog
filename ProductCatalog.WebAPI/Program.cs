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

//app.MapGet("api/products", (int id) => Results.Json(id));

app.SetUpCRUDEndpoints();


app.Run();
