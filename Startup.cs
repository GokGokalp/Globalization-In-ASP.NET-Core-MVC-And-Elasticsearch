using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCoreAndESearch.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ASPNETCoreAndESearch
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
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddMvc()
                     .AddViewLocalization();

            services.AddSingleton<ConnectionSettings>(x => new ConnectionSettings(new Uri("http://localhost:9200")));
            services.AddTransient<IProductService, ProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRouter(routes =>
            {
                routes.MapMiddlewareRoute("{culture=tr-TR}/{*mvcRoute}", _app =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("tr-TR"),
                        new CultureInfo("en-US")                                   
                    };

                    var requestLocalizationOptions = new RequestLocalizationOptions
                    {
                        DefaultRequestCulture = new RequestCulture("tr-TR"),
                        SupportedCultures = supportedCultures,
                        SupportedUICultures = supportedCultures
                    };

                    requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                    _app.UseRequestLocalization(requestLocalizationOptions);

                    _app.UseMvc(mvcRoutes =>
                    {
                        mvcRoutes.MapRoute(
                            name: "default",
                            template: "{culture=tr-TR}/{controller=Home}/{action=Index}/{id?}");
                    });
                });
            });
        }
    }
}