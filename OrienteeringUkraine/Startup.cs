using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace OrienteeringUkraine
{
    public class Startup
    {
        private IConfigurationRoot confString;

        public Startup(IHostEnvironment hostEnv)
        {
            confString = new ConfigurationBuilder().SetBasePath(hostEnv.ContentRootPath).AddJsonFile("dbsettings.json").Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFContext>(options => options.UseSqlServer(confString.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("OrienteeringUkraine")));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            this.SeedDB(app);
        }

        public void SeedDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                EFContext context = serviceScope.ServiceProvider.GetService<EFContext>();

                if (!context.Regions.Any())
                {
                    context.Regions.AddRange(
                        new Region { Name = "АР Крым" },
                        new Region { Name = "Винницкая область" },
                        new Region { Name = "Волынская область" },
                        new Region { Name = "Днепропетровская область" },
                        new Region { Name = "Донецкая область" },
                        new Region { Name = "Житомирская область" },
                        new Region { Name = "Закарпатская область" },
                        new Region { Name = "Запорожская область" },
                        new Region { Name = "Ивано-Франковская область" },
                        new Region { Name = "Город Киев" },
                        new Region { Name = "Киевская область" },
                        new Region { Name = "Кировоградская область" },
                        new Region { Name = "Луганская область" },
                        new Region { Name = "Львовская область" },
                        new Region { Name = "Николаевская область" },
                        new Region { Name = "Одесская область" },
                        new Region { Name = "Полтавская область" },
                        new Region { Name = "Ровенская область" },
                        new Region { Name = "Город Севастополоть" },
                        new Region { Name = "Сумская область" },
                        new Region { Name = "Тернопольская область" },
                        new Region { Name = "Харьковская область" },
                        new Region { Name = "Херсонская область" },
                        new Region { Name = "Хмельницкая область" },
                        new Region { Name = "Черкасская область" },
                        new Region { Name = "Черниговская область" },
                        new Region { Name = "Черновицкая область" }
                        );

                }

                context.SaveChanges();
            }
        }
    }
}
