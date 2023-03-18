using System.Reflection;
using DevEventsApi.Persistence;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
  j => j.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddEndpointsApiExplorer();
ConfigureServices(builder);
ConfigureSwagger(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
else
{
  app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
  builder.Services.AddDbContext<DatabaseContext>(
    opts => opts.UseSqlite(builder.Configuration.GetConnectionString("Database"))
  );
}

void ConfigureSwagger(WebApplicationBuilder builder)
{
  builder.Services.AddSwaggerGen(opts =>
  {
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
      Version = "v1",
      Title = "DevEventsAPI",
      Contact = new OpenApiContact()
      {
        Name = "Matheus Adami Rodrigues",
        Email = "matheus.adami.dev@gmail.com",
        Url = new Uri("https://www.linkedin.com/in/matheus-adami/")
      }
    });

    // Enable XML comments in the Controllers / Actions.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
  });
}