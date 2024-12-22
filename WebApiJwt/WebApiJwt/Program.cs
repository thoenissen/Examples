using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApiJwt;

/// <summary>
/// Main class
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main entry
    /// </summary>
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        var issuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
        var issuerSigningKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

        // Adding authentication through a JWT which is passed at header argument of type Bearer. This scheme is used as default for all routes.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                                      {
                                          options.TokenValidationParameters = new TokenValidationParameters
                                                                              {
                                                                                  ValidateIssuer = true,
                                                                                  ValidateAudience = true,
                                                                                  ValidateLifetime = true,
                                                                                  ValidateIssuerSigningKey = true,
                                                                                  ValidIssuer = issuer,
                                                                                  ValidAudience = issuer,
                                                                                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
                                                                                  RoleClaimType = ClaimTypes.Role
                                                                              };
                                      })

                        // Additionally adding windows authentication. This scheme is only used explicitly for the Single Sign-On (SSO) routes.
                        .AddNegotiate();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
                                       {
                                           c.SwaggerDoc("v1",
                                                        new OpenApiInfo
                                                        {
                                                            Title = "WebApiJwt",
                                                            Version = "v1"
                                                        });
                                           c.AddSecurityDefinition("Bearer",
                                                                   new OpenApiSecurityScheme
                                                                   {
                                                                       Name = "Authorization",
                                                                       Type = SecuritySchemeType.ApiKey,
                                                                       Scheme = "Bearer",
                                                                       BearerFormat = "JWT",
                                                                       In = ParameterLocation.Header,
                                                                       Description = "JWT Authorization header using the Bearer scheme.",
                                                                   });
                                           c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                                    {
                                                                        [
                                                                            new OpenApiSecurityScheme 
                                                                            {
                                                                                Reference = new OpenApiReference
                                                                                                {
                                                                                                    Type = ReferenceType.SecurityScheme,
                                                                                                    Id = "Bearer"
                                                                                                }
                                                                            }
                                                                        ] = []
                                                                       }
                                                                    );
                                       });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}