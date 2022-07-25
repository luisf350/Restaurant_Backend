using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurant.Backend.Common.Enums;
using Restaurant.Backend.CommonApi.Extensions;
using Restaurant.Backend.CommonApi.Utils;
using Restaurant.Backend.Entities.Context;

namespace Restaurant.Backend.Account
{
    [ExcludeFromCodeCoverage]
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
            services.AddCors();
            services.AddControllers();
            services.AddResponseCaching();

            // Swagger Documentation
            services.AddSwaggerDocumentation();

            // Add JwtAuthentication
            services.AddJwtAuthentication(JwtCreationUtil.GetJwtToken(Configuration).Result);

            // Set configuration for Entity Framework
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(StringConnectionUtil.GetStringConnection(Configuration).Result));

            // Dependency Injection
            services.AddJwtAuthentication(Microservice.Account);

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.AddSwagger();
            }

            // Global Error Handling
            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseResponseCaching();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
