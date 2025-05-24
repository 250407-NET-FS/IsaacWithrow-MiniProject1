using Microsoft.EntityFrameworkCore;
using MiniAPI.Data;
using MiniAPI.Services;
using MiniAPI.Services.Games.Commands;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<CreateGame.Handler>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AzurePolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

//swagger
//Adding swagger support
builder.Services.AddEndpointsApiExplorer();

//Modifying this AddSwaggerGen() call to allow us to test/debug our Auth scheme setup in swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddDbContext<MiniAPIContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowReactApp");
    Console.WriteLine("allowed dev");
    
}
else
{
    app.UseCors("AllowFrontend");

    app.UseExceptionHandler("/Error");
    app.UseHsts();

}

app.UseCors("AzurePolicy");

app.UseHttpsRedirection();

app.MapControllers();

// implementing and using a service directly:
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<MiniAPIContext>();
    // will move existing migrations if there are any 
    // that haven't been applied yet
    await context.Database.MigrateAsync();
    // creating seed records
    //await DbInitializer.SeedData(context);
}
catch (System.Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migrations");
}

app.Run();
