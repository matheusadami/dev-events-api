using System.Text;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DevEventsApi.Extensions;
using DevEventsApi.Persistence;
using DevEventsApi.Services.Interfaces;
using DevEventsApi.Services;
using DevEventsApi.Config;
using DevEventsApi.Persistence.Interceptors;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);
ConfigureSwagger(builder);

var app = builder.Build();
ConfigureMiddlewares(app);

app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
  builder.Services.AddHttpContextAccessor();
  builder.Services.AddControllers().AddJsonOptions(
    opts =>
    {
      opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
  );

  ConfigureDatabase(builder);
  ConfigureAuthScheme(builder);

  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSingleton<ITokenService, TokenService>();
  builder.Services.AddSingleton<IHasherService, HasherService>();

  builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(AuthSettings.Key));
  builder.Services.Configure<HashingSettings>(builder.Configuration.GetSection(HashingSettings.Key));
}

void ConfigureDatabase(WebApplicationBuilder builder)
{
  // Interceptors
  builder.Services.AddSingleton<AuditableEntitiesInterceptor>();

  // Database Context
  builder.Services.AddDbContext<DatabaseContext>();
}

void ConfigureAuthScheme(WebApplicationBuilder builder)
{
  var authSettings = builder.Configuration.GetSection(AuthSettings.Key).Get<AuthSettings>();
  var key = Encoding.ASCII.GetBytes(authSettings.JWTSecretKey);

  builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(
    x =>
    {
      x.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
      x.SaveToken = true;
      x.TokenValidationParameters = new()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
      };
    }
  );
}

void ConfigureMiddlewares(WebApplication app)
{
  app.UseHttpsRedirection();
  app.UseAuthentication();
  app.UseAuthorization();
  app.UseLogger();
  app.MapControllers();

  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }
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

    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
      In = ParameterLocation.Header,
      Description = "Please enter a valid JWT token",
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      BearerFormat = "JWT",
      Scheme = "Bearer"
    });

    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme {
          Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        },
        new string[] {}
      }
    });
  });
}