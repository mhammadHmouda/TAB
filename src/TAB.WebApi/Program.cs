using Serilog;
using TAB.Application;
using TAB.Infrastructure;
using TAB.Persistence;
using TAB.WebApi;
using TAB.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    }
);

builder.Services.AddWebApi().AddApplication().AddInfrastructure().AddPersistence();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
