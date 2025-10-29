using System.Reflection;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Interfaces.Http.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LeafBidAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080); // match Docker container port
        });

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddRouting();
        
        // Automatically register AutoMapper profiles across the assembly
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Automatically register Repositories, Validators, and other services via Scrutor
        builder.Services.Scan(scan => scan
            .FromApplicationDependencies(a => a.FullName != null && a.FullName.StartsWith("LeafBidAPI"))
            // Repositories
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
            .AsSelfWithInterfaces()
            .WithScopedLifetime()
            // Validators
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Validator")))
            .AsSelfWithInterfaces()
            .WithScopedLifetime()
            // Hashers, Filters, etc.
            .AddClasses(c => c.Where(t => t.Name.EndsWith("Hasher") || t.Name.EndsWith("Filter")))
            .AsSelfWithInterfaces()
            .WithScopedLifetime()
        );
        
        // Set-up versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        // Set-up versioning for Swagger
        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // format: 'v'major[.minor][.patch]
            options.SubstituteApiVersionInUrl = true;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "LeafBidAPI", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            c.SchemaFilter<EnumSchemaFilter>();
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LeafBidAPI v1"));
        }
        
        // Configure HTTPS if not in development
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();
        app.UseRouting();
        app.MapControllers();

        app.Run();
    }
}