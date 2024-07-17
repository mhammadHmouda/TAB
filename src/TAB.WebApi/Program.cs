using Serilog;
using TAB.Application;
using TAB.Infrastructure;
using TAB.Persistence;
using TAB.WebApi;
using TAB.WebApi.Extensions;
using TAB.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    }
);

var configuration = builder.Configuration;

builder.Services.AddWebApi().AddApplication().AddInfrastructure().AddPersistence(configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
