using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swapp.Data;

namespace FindMe.Web.App
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new SwpDateTimeConverter());
                });

            services.AddDbContext<AppDbContext>();
            services.AddSingleton<WebDbRepository>();

            services.AddTransient<AppMigrationSeedManager>();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            WebDbRepository repo,
            AppMigrationSeedManager seeder)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

                loggerFactory.AddDebug(LogLevel.Information);
                loggerFactory.AddDatabase(LogLevel.Error);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                loggerFactory.AddDebug(LogLevel.Error);
                loggerFactory.AddDatabase(LogLevel.Critical);
            }

            app.UseStaticFiles();

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=App}/{action=Index}/{id?}");
                    //defaults: "App/Index");

                }
            );


            if (this.Configuration.IsMigration())
            {
                seeder.MigrateAndSeed();
            }
        }
    }
}
