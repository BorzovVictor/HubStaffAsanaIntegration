using HI.Api.Extensions;
using HI.Api.Services;
using HI.Asana;
using HI.Hubstaff;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HI.Api
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

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=db.sqlite"));
            
            #region singleton configurations

            var hubstaffConfig = new HubstaffSettings();
            Configuration.Bind("hubstaff", hubstaffConfig);
            services.AddSingleton(hubstaffConfig);
            
            var asanaConfig = new AsanaSettings();
            Configuration.Bind("asana", asanaConfig);
            services.AddSingleton(asanaConfig);

            #endregion
            
            // configure strongly typed settings objects
            //services.Configure<AsanaSettings>(Configuration.GetSection("asana"));
            //services.Configure<HubstaffSettings>(Configuration.GetSection("hubstaff"));

            services.AddProjectServices();
            
            services.AddHostedService<HI.Api.Services.BackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}