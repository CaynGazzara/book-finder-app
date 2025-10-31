using BookFinder.API.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookFinder API",
        Version = "v1",
        Description = "API para busca de livros usando Google Books API",
        Contact = new OpenApiContact
        {
            Name = "BookFinder Team",
            Email = "dev@bookfinder.com"
        }
    });
});

// Configure HttpClient and Services
builder.Services.AddHttpClient<IBookService, GoogleBooksService>(client =>
{
    client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
    client.DefaultRequestHeaders.Add("User-Agent", "BookFinderApp/1.0");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// CORS CONFIGURATION - PERMISSIVA PARA DESENVOLVIMENTO
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookFinder API v1");
        c.RoutePrefix = "swagger";
    });
}

// USE CORS - IMPORTANTE: Deve vir antes de UseAuthorization e MapControllers
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();