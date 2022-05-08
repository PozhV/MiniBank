using MiniBank.Core;
using MiniBank.Data;
using MiniBank.Web.Middlewares;
using MiniBank.Web.HostedServices;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;

namespace MiniBank.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Audience = "api";
                options.Authority = "https://demo.duendesoftware.com";
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniBank.Web", Version = "v1" });
                options.AddSecurityDefinition("oauth2",
                    new OpenApiSecurityScheme() {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows {
                            ClientCredentials = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri("https://demo.duendesoftware.com/connect/token"),
                                Scopes = new Dictionary<string, string>()
                            }
                        }
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = SecuritySchemeType.OAuth2.GetDisplayName()
                            }
                        },
                        new List<string>()
                    }
                });
            });
            services.AddHostedService<MigrationHostedService>();
            services.AddData(Configuration).AddCore();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniBank.Web v1"));
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
