using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Markit.Api.Interfaces.Managers;
using Markit.Api.Interfaces.Repositories;
using Markit.Api.Interfaces.Utils;
using Markit.Api.Managers;
using Markit.Api.Mappers;
using Markit.Api.Models.Dtos;
using Markit.Api.Repositories;
using Markit.Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Markit.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Markit.Api",
                    Version = "v1"
                });
                
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
 
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(securitySchema, new[] { "Bearer" });
                c.AddSecurityRequirement(securityRequirement);
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;

                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")))
                    };
                });
            
            services.AddControllers();
            
            services.AddAutoMapper(typeof(Startup).Assembly);

            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddSingleton<IUserManager, UserManager>()
                .AddSingleton<IDatabaseUtil, DatabaseUtil>()
                .AddSingleton<IPasswordUtil, PasswordUtil>()
                .AddSingleton<IStoreManager, StoreManager>()
                .AddSingleton<IStoreRepository, StoreRepository>()
                .AddSingleton<IItemManager, ItemManager>()
                .AddSingleton<IItemRepository, ItemRepository>()
                .AddSingleton<ITagManager, TagManager>()
                .AddSingleton<ITagRepository, TagRepository>()
                .AddSingleton<IListManager, ListManager>()
                .AddSingleton<IListRepository, ListRepository>()
                .AddSingleton<IRatingRepository, RatingRepository>()
                .AddSingleton<IRatingsManager, RatingsManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            
            app.UseSwaggerUI(c=>    
            {    
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Markit.Api");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}