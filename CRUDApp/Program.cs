using CRUDApp.DataAccess;
using CRUDApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureLogging((context, logger) =>
{
    logger.AddConfiguration(context.Configuration.GetSection("Logging"));
    logger.AddConsole();
    logger.AddDebug();
    logger.AddEventSourceLogger();
    logger.AddNLog();
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Adding services for JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer( options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
        ValidAudience = builder.Configuration["JWTSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

//register DbContext in the DI container
builder.Services.AddDbContext<PSQLDBContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("EmployeeDBConn"))
);

//register a service with a scoped lifetime i.e. instance is created once per client request(HTTP request)
builder.Services.AddScoped<IDataAccessProvider, DataAccessProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();


    app.UseExceptionHandler(error =>
    {
        error.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                app.Logger.LogError($"{DateTime.Now}: Something went wrong in the {contextFeature.Error}");

                await context.Response.WriteAsync(new ErrorModel
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please Try Again Later"
                }.ToString());
            }
        });
    });


    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
