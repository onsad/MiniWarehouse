using MiniWarehouse.Repository;
using MiniWarehouse.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<DataRepository>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<ProductService>();

builder.Services.AddControllers();
// Development CORS policy - allow the Blazor WASM client to call this API during development.
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<MiniWarehouse.Server.MiddleWare.ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS policy for development
app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
