using Application.Common.Behaviors;
using Application.Products.Commands.CreateProduct;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.OpenApi.Models;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
});

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET Clean Architecture API",
        Version = "v1",
        Description = "A sample API demonstrating Clean Architecture with CQRS and DDD principles.",
        Contact = new OpenApiContact { Name = "Fernando Terra", Url = new Uri("https://github.com/fernando-terra") }
    });
    c.EnableAnnotations();
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Clean Architecture v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
