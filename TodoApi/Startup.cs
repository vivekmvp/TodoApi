using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Extensions;
using TodoApi.Models;


namespace TodoApi
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
            services.AddControllers();


            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(4, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
                        
            services.AddVersionedApiExplorer(c =>
            {
                c.GroupNameFormat = "'v'VVV";
                c.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API v1.0"
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "ToDo API v2.0"
                });
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "v3",
                    Title = "ToDo API v3.0"
                });
                c.SwaggerDoc("v4", new OpenApiInfo
                {
                    Version = "v4",
                    Title = "ToDo API v4.0"
                });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            //For In-Memory Db Operations
            services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //*******************
                //Put here so that swagger will be available in development
                //And it's unavailable when you deployed to Azure
                //app.UseSwaggerWithVersioning();
                //*******************
            }

            //Putting this here for testing purpose only
            app.UseSwaggerWithVersioning();

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
