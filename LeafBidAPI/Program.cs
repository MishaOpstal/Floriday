using System.Reflection;
using LeafBidAPI.Data;
using LeafBidAPI.Filters;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LeafBidAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var allowedOrigins = "_allowedOrigins";
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080); // match Docker container port
        });

        // Add services to the container.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: allowedOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddRouting();
        builder.Services.AddHttpClient();
        
        //TODO: Misha uncomment dit als jij het goed vindt, deze code is direct overgenomen vanuit Brightspace
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
            
        builder.Services.AddScoped<RoleManager<IdentityRole>>();
        builder.Services.AddTransient<IEmailSender<User>, DummyEmailSender>();

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
            
            //security definitie toevoegen
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Please enter a valid token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            
            // security requierment toevoegen
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                new List<string>()
                }

            });
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
        
        //app.UseAuthentication();
        app.UseAuthorization();
        //app.MapIdentityApi<[NaamVanIdentityKlasse]>();
        app.UseRouting();
        app.MapControllers();
        app.UseStaticFiles();
        app.UseCors(allowedOrigins);
        
        app.Run();
    }
}