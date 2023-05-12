using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ophen.JobProcess.API.Models;
using Ophen.JobProcess.ApplicationServices;
using Ophen.JobProcess.ApplicationServices.ProcessingStrategy;
using Ophen.JobProcess.ApplicationServices.Services;
using Ophen.JobProcess.Common;
using Ophen.JobProcess.DomainServices;
using Ophen.JobProcess.Infraestructure;
using Ophen.JobProcess.Infraestructure.Repositories;

namespace Ophen.JobProcess.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<JobProcessContext>(opt => opt.UseInMemoryDatabase("JobPrecessDb"));

            services.Configure<DictionaryConfig>(Configuration.GetSection("DictionaryConfig"));

            services.AddScoped<IJobRep, JobRep>();
            services.AddScoped<IJobItemRep, JobItemRep>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobItemService, JobItemService>();
            services.AddScoped<IJobDataProcessingService, JobDataProcessingService>();
            services.AddScoped<IStrategyContext, StrategyContext>();
            services.AddScoped<IStrategy, BulkStrategy>();
            services.AddScoped<IStrategy, BatchStrategy>();
            services.AddScoped<BulkStrategy>();
            services.AddScoped<BatchStrategy>();
            services.AddScoped<IExternalProcessingService, MockExternalProcessingService>();

            services.AddAutoMapper(c => c.AddProfile<MappingProfile>(), typeof(Startup));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ophen.JobProcess.API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);                
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ophen.JobProcess.API");
            });
        }
    }
}
