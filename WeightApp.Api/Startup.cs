using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NSwag;

using WeightApp.Api.Services;
using WeightApp.Db;

namespace WeightApp.Api
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
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddSingleton<IWeightCalculator, WeightCalculator>();
            services.AddSingleton<IRepository, Repository>();
            services.AddSingleton<IHashProvider>(new HashProvider());
            services.AddSingleton<Func<IDbConnection>>(
                () => new SqlConnection(Configuration.GetConnectionString("App"))
                );

            services.AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerDocument(
                p => p.AddSecurity(
                    "BasicAuthentication", 
                    Enumerable.Empty<string>(), 
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.Basic
                    }).Title = "WeightApp"
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
