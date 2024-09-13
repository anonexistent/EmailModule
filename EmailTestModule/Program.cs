using EfiritPro.Retail.AuthModule.Persistence;
using EfiritPro.Retail.Packages.Authorization.Interfaces;
using EfiritPro.Retail.Packages.Rabbit.Interfaces;
using EfiritPro.Retail.Packages.Utils;
using EmailTestModule.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    DotEnvService.LoadEnvironmentFromFile(Path.Combine(builder.Environment.ContentRootPath, "Development.env"));
}

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "EfiritPro.Retail.EmailTest.Api", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var emailTestDbConnectionString = Environment.GetEnvironmentVariable("EMAIL_TEST_DB") ??
                              throw new InvalidOperationException("Connection string EMAIL_TEST_DB not found.");
var originsString = Environment.GetEnvironmentVariable("ORIGINS") ??
              throw new InvalidOperationException("string ORIGIN not found");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(originsString.Split(';'))
            // .AllowAnyOrigin()
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services
    .AddDbContext<EmailTestDbContext>(options => options
        .UseNpgsql(emailTestDbConnectionString)
        .UseSnakeCaseNamingConvention()
        .UseLazyLoadingProxies())
    //.AddRabbit<AuthDbContext, RabbitEventHandler>()
    //.AddSingleton<PasswordService>()
    .AddScoped<ResetPasswordEmailService>();

var app = builder.Build();

app
    .UseSwagger(x =>
     {
         x.RouteTemplate = "emailTestModule/swagger/{documentname}/swagger.json";
     })
    .UseSwaggerUI(x =>
    {
        x.RoutePrefix = "emailTestModule/swagger";
    });

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<EmailTestDbContext>();
    dbContext?.Database.Migrate();
}

app.Run();
