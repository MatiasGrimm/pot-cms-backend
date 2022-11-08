using PotShop.API.Data;
using PotShop.API.Models;
using PotShop.API.Services;
using PotShop.API.ViewModels.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("PotShopAPI")));

            // configurations
            services.Configure<SiteOptions>(Configuration.GetSection(nameof(SiteOptions)));

            AuthenticationStartup.Configure(Configuration, services);

            services.AddCors();
            services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

            // services and models
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IAuthModel, AuthModel>();
            services.AddSingleton<IMailService, MailService>();

            if (Environment.IsDevelopment())
            {
                services.AddSingleton<IMailSenderService, DummyMailSenderService>();
            }
            else
            {
                // production services
            }

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new GuidLowercaseConverter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DefaultData.PopulateAccounts(services).Wait();
        }
    }
}
