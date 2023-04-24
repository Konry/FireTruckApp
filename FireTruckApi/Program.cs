// Copyright (c) Jan Philipp Luehrig. All rights reserved.
// These files are licensed to you under the MIT license.

using FireTruckApi.DataHandling;
using FireTruckApp.DataLoader;
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
builder.Services.AddSingleton<IDataStorage, DataStorage>();
builder.Services.AddSingleton<IExcelDataLoader, ExcelDataLoader>();
builder.Services.AddHealthChecks().AddCheck<SampleHealthCheck>("Sample");

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseHealthChecks("/healthz");
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
