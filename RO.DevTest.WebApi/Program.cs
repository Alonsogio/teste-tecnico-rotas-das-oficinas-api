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
using FluentValidation.AspNetCore;
using RO.DevTest.Application.Validators.Products;
using RO.DevTest.Application.Validators.Clients;

namespace RO.DevTest.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
        .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
    })

    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        config.RegisterValidatorsFromAssemblyContaining<CreateClientCommandValidator>();

    });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies();

        // Add Mediatr to program
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly,

                // clients
                typeof(GetClientsQueryHandler).Assembly,
                typeof(GetClientByIdQueryHandler).Assembly,
                typeof(UpdateClientCommandHandler).Assembly,
                typeof(DeleteClientCommandHandler).Assembly,

                // products
                typeof(GetProductsQueryHandler).Assembly,
                typeof(GetProductByIdQueryHandler).Assembly,
                typeof(UpdateProductCommandHandler).Assembly,
                typeof(DeleteProductCommandHandler).Assembly
            );
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
