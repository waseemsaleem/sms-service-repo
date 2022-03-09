using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ApiService.Models;
using ApiService.Services;
using ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace ApiService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<accountdbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{Configuration.GetValue<string>("Redis:Server")}:{Configuration.GetValue<int>("Redis:Port")}";
            });
            // Add JWT configuration
            var bindJwtSettings = new JwtSettings(Configuration);
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            services.AddSingleton(bindJwtSettings);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateActor = bindJwtSettings.ValidateIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidateLifetime = bindJwtSettings.RequireExpirationTime,
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1)
                };
            });
            // End JWT configuration

            services.AddControllers().AddNewtonsoftJson(); ;
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiService", Version = "v1" });
            });

            services.AddScoped<IPhoneService, PhoneService>();
            services.AddScoped<IAccountService, AccountService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiService v1"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
