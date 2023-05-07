// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using System.IO.Abstractions;
using FireTruckApi;
using FireTruckApi.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Logger logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).WriteTo.Console().Enrich
    .FromLogContext().CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.

logger.Information("Startup Firetruck Api");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<IDataStorage, DataStorage>();
builder.Services.AddSingleton<IExcelDataLoader, ExcelDataLoader>();

builder.Services.AddHealthChecks().AddCheck<ApiHealthCheck>("Api", failureStatus: HealthStatus.Degraded);
builder.Services.AddProblemDetails();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // To preserve the default behavior, capture the original delegate to call later.
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            // Perform logging here.
            logger.Error("Error in api access");

            // Invoke the default behavior, which produces a ValidationProblemDetails
            // response.
            // To produce a custom response, return a different implementation of
            // IActionResult instead.
            return builtInFactory(context);
        };
    });

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseHealthChecks("/healthz"); // needed for docker
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
} else
{
    // todo some example shows this, and some others the current included stuff, figure out how this is working
    // app.UseExceptionHandler("/error");
}
app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
