using ProductCatalog.WebAPI.Endpoints;
using ProductCatalog.WebAPI.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddNpgProductRepository();
builder.Services.AddImageStorage();
builder.Services.AddMapper();
builder.Services.AddProductServices();

builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));


WebApplication app = builder.Build();

string staticPrefix = "/api/products/";

app.UseCRUDEndpoints(staticPrefix);
app.UseFileEndpoints(staticPrefix);
app.UsePageEndpoints(staticPrefix);

app.Run();
