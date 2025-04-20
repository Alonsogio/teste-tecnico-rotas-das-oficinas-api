using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence.IoC;
using RO.DevTest.Application.Contracts.Persistence.Repositories;
using RO.DevTest.Persistence.Repositories;
using RO.DevTest.Application.Features.Clients.Queries.GetClientsQuery;
using RO.DevTest.Application.Features.Clients.Queries.GetClientByIdQuery;
using RO.DevTest.Application.Features.Clients.Commands.UpdateClientCommand;
using RO.DevTest.Application.Features.Clients.Commands.DeleteClientCommand;

namespace RO.DevTest.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();

        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies();

        // Add Mediatr to program
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly,
                typeof(GetClientsQueryHandler).Assembly,
                typeof(GetClientByIdQueryHandler).Assembly,
                typeof(UpdateClientCommandHandler).Assembly,
                typeof(DeleteClientCommandHandler).Assembly
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
