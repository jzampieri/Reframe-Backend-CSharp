using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Reframe.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ReframeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Explorer para o Swagger separar por versão
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Reframe API v1",
        Version = "v1",
        Description = "API para upskilling/reskilling com IA - versão 1"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Reframe API v2",
        Version = "v2",
        Description = "API para upskilling/reskilling com IA - versão 2 (experimental)"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Reframe API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Reframe API v2");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();