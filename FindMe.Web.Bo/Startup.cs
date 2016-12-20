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
        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new SwpDateTimeConverter());
                });

            if (_env.IsDevelopment())
            {
                services.AddTransient<IMailService, DevMailService>();
            }
            else
            {
                services.AddTransient<IMailService, MailService>();
            }

            services.AddDbContext<AppDbContext>();
            services.AddSingleton<WebDbRepository>();

            services.AddTransient<AppMigrationSeedManager>();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
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
                }
            );


            if (this.Configuration.IsMigration())
            {
                seeder.MigrateAndSeed();
            }
        }
    }
}
