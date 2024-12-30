using System;
using System.Collections.Generic;
using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RiverBooks.Books;
using RiverBooks.OrderProcessing;
using RiverBooks.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument()
    .AddAuthenticationJwtBearer(options => options.SigningKey = builder.Configuration["Auth:JwtSecret"])
    .AddAuthorization();

// Register module services
var mediatorAssemblies = new List<Assembly>();
builder.Services.AddBookServices(builder.Configuration, mediatorAssemblies);
builder.Services.AddUserServices(builder.Configuration, mediatorAssemblies);
builder.Services.AddOrderProcessingServices(builder.Configuration, mediatorAssemblies);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatorAssemblies.ToArray()));


var app = builder.Build();

app.UseAuthentication().UseAuthorization();

app.UseFastEndpoints().UseSwaggerGen();

app.Run();
