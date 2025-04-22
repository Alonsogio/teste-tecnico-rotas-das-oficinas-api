using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence.IoC;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Persistence.Repositories;
using RO.DevTest.Application.Features.Clients.Queries.GetClientsQuery;
using RO.DevTest.Application.Features.Clients.Queries.GetClientByIdQuery;
using RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand;
using RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand;
using RO.DevTest.Application.Features.Products.Queries.GetProductsQuery;
using RO.DevTest.Application.Features.Products.Queries.GetProductByIdQuery;
using RO.DevTest.Application.Features.Products.Commands.UpdateProductCommand;
using RO.DevTest.Application.Features.Products.Commands.DeleteProductCommand;
using RO.DevTest.Application.Features.Sales.Commands.DeleteSaleCommand;
using RO.DevTest.Application.Validators.Products;
using RO.DevTest.Persistence.Contexts;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;

namespace RO.DevTest.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // JWT Settings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


        // Controllers + JSON options
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
            });

        // FluentValidation
        builder.Services.AddFluentValidationAutoValidation()
                        .AddFluentValidationClientsideAdapters()
                        .AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Repositórios
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();

        // Dependências da aplicação
        builder.Services.InjectPersistenceDependencies()
                        .InjectInfrastructureDependencies();

        // Banco de Dados
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        });

        // MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,

                // Clients
                typeof(GetClientsQueryHandler).Assembly,
                typeof(GetClientByIdQueryHandler).Assembly,
                typeof(UpdateClientCommandHandler).Assembly,
                typeof(DeleteClientCommandHandler).Assembly,

                // Products
                typeof(GetProductsQueryHandler).Assembly,
                typeof(GetProductByIdQueryHandler).Assembly,
                typeof(UpdateProductCommandHandler).Assembly,
                typeof(DeleteProductCommandHandler).Assembly,

                // Sales
                typeof(DeleteSaleCommandHandler).Assembly,

                // Login
                typeof(LoginCommandHandler).Assembly
            );
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
public static class JwtExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfigurationSection jwtSettings)
    {
        var key = jwtSettings["Key"];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("A chave JWT não foi fornecida");
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }
}
