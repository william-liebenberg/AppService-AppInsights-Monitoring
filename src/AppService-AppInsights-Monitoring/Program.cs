using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// turn the usual 400 NOT FOUND into 200 OK when the load balancer hits the root of the site every 5 minutes (for Always On setting)
// see why Always On generates this traffic: https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal#configure-general-settings
app.MapGet("/", () => { return Results.Ok(); })
    .ExcludeFromDescription();

// simple endpoint to track request, event, and trace
app.MapGet("/test", ([FromServices]TelemetryClient tc) =>
{
    tc.TrackEvent("TEST EVENT");
    tc.TrackTrace("Hi! Ignore me, I'm just testing stuff !!!");

    return "Testing!";
})
.WithName("GetTest")
.WithOpenApi();

app.Run();
