using ProductCatalog.WebAPI.Endpoints;
using ProductCatalog.WebAPI.Extentions;

const string c_staticPrefix = "/api/products/";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddNpgProductRepository();
builder.Services.AddImageStorage();
builder.Services.AddMapper();
builder.Services.AddProductServices();
builder.Services.AddMediatR();
builder.Services.AddProductsSwagger();

WebApplication app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCRUDEndpoints(c_staticPrefix);
app.UseFileEndpoints(c_staticPrefix);
app.UsePageEndpoints(c_staticPrefix);

app.Run();
