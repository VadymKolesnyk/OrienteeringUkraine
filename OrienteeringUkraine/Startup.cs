using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using DataLayer;
using DataLayer.Tables;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
            services.AddDbContext<EFContext>(options => options.UseSqlServer(confString.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("OrienteeringUkraine")));
            services.AddScoped<EFDataManager>();
            services.AddScoped<IDataManager, TempDataManager>();
            services.AddMvc();
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

            app.UseAuthentication();   
            app.UseAuthorization();     

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //SeedDB(app);
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
                        new Region { Name = "Винницкая" },
                        new Region { Name = "Волынская" },
                        new Region { Name = "Днепропетровская" },
                        new Region { Name = "Донецкая" },
                        new Region { Name = "Житомирская" },
                        new Region { Name = "Закарпатская" },
                        new Region { Name = "Запорожская" },
                        new Region { Name = "Ивано-Франковская" },
                        new Region { Name = "Город Киев" },
                        new Region { Name = "Киевская" },
                        new Region { Name = "Кировоградская" },
                        new Region { Name = "Луганская" },
                        new Region { Name = "Львовская" },
                        new Region { Name = "Николаевская" },
                        new Region { Name = "Одесская" },
                        new Region { Name = "Полтавская" },
                        new Region { Name = "Ровенская" },
                        new Region { Name = "Город Севастополоть" },
                        new Region { Name = "Сумская" },
                        new Region { Name = "Тернопольская" },
                        new Region { Name = "Харьковская" },
                        new Region { Name = "Херсонская" },
                        new Region { Name = "Хмельницкая" },
                        new Region { Name = "Черкасская" },
                        new Region { Name = "Черниговская" },
                        new Region { Name = "Черновицкая" }
                        );

                }

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role { Name = "admin" },
                        new Role { Name = "moderator" },
                        new Role { Name = "organizer" },
                        new Role { Name = "sportsman" }
                        );
                }

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(
                        new Club { Name = "Азимут" },
                        new Club { Name = "Odessa Team" },
                        new Club { Name = "Клуб веб-дизайнеров" },
                        new Club { Name = "Север" },
                        new Club { Name = "Юг" },
                        new Club { Name = "Запад" },
                        new Club { Name = "Восток" }
                        );
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            Name = "Super",
                            Surname = "Admin",
                            BirthDate = new DateTime(2000, 1, 1),
                            RoleId = 1,
                            RegionId = 1,
                            ClubId = 0
                        },
                        new User
                        {
                            Name = "Вадим",
                            Surname = "Колесник",
                            BirthDate = new DateTime(2001, 2, 19),
                            RoleId = 2,
                            RegionId = 17,
                            ClubId = 2
                        },
                        new User
                        {
                            Name = "Екатерина",
                            Surname = "Кубышка",
                            BirthDate = new DateTime(2001, 9, 23),
                            RoleId = 2,
                            RegionId = 17,
                            ClubId = 3
                        },
                        new User
                        {
                            Name = "Сергей",
                            Surname = "Переяславский",
                            BirthDate = new DateTime(2000, 11, 22),
                            RoleId = 3,
                            RegionId = 11,
                            ClubId = 2
                        },
                        new User
                        {
                            Name = "Александр",
                            Surname = "Дзюбчик",
                            BirthDate = new DateTime(2001, 3, 24),
                            RoleId = 3,
                            RegionId = 3,
                            ClubId = 3
                        },
                        new User
                        {
                            Name = "Биба",
                            Surname = "Бобович",
                            BirthDate = new DateTime(2005, 2, 20),
                            RoleId = 3,
                            RegionId = 17,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Максим",
                            Surname = "Гапонюк",
                            BirthDate = new DateTime(2000, 12, 4),
                            RoleId = 3,
                            RegionId = 3,
                            ClubId = 3
                        },
                        new User
                        {
                            Name = "Гибон",
                            Surname = "Пожилой",
                            BirthDate = new DateTime(1900, 2, 2),
                            RoleId = 4,
                            RegionId = 3,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Тарас",
                            Surname = "Рачук",
                            BirthDate = new DateTime(1998, 7, 28),
                            RoleId = 4,
                            RegionId = 25,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Сергей",
                            Surname = "Поникаров",
                            BirthDate = new DateTime(1982, 2, 23),
                            RoleId = 4,
                            RegionId = 25,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Надежда",
                            Surname = "Сердюк",
                            BirthDate = new DateTime(1953, 6, 25),
                            RoleId = 4,
                            RegionId = 10,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Олег",
                            Surname = "Скляр",
                            BirthDate = new DateTime(1985, 5, 25),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Елена",
                            Surname = "Стрыжак",
                            BirthDate = new DateTime(1963, 10, 7),
                            RoleId = 4,
                            RegionId = 17,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Галина",
                            Surname = "Строчук",
                            BirthDate = new DateTime(1977, 7, 26),
                            RoleId = 4,
                            RegionId = 18,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Сергей",
                            Surname = "Тарасенко",
                            BirthDate = new DateTime(1972, 4, 28),
                            RoleId = 4,
                            RegionId = 22,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Наталия",
                            Surname = "Уманец",
                            BirthDate = new DateTime(1978, 1, 19),
                            RoleId = 4,
                            RegionId = 25,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Александр",
                            Surname = "Уфимцев",
                            BirthDate = new DateTime(1957, 10, 29),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Андрей",
                            Surname = "Алёшин",
                            BirthDate = new DateTime(1996, 12, 9),
                            RoleId = 4,
                            RegionId = 7,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Владимир",
                            Surname = "Бондаренко",
                            BirthDate = new DateTime(1959, 7, 28),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Владислав",
                            Surname = "Вовк",
                            BirthDate = new DateTime(1968, 11, 21),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Артур",
                            Surname = "Ганилов",
                            BirthDate = new DateTime(1968, 5, 12),
                            RoleId = 4,
                            RegionId = 6,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Виталий",
                            Surname = "Кличко",
                            BirthDate = new DateTime(1999, 9, 21),
                            RoleId = 4,
                            RegionId = 17,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Евгений",
                            Surname = "Гевел",
                            BirthDate = new DateTime(1956, 4, 2),
                            RoleId = 4,
                            RegionId = 4,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Мария",
                            Surname = "Гудак",
                            BirthDate = new DateTime(1988, 8, 12),
                            RoleId = 4,
                            RegionId = 26,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Руслан",
                            Surname = "Шимчук",
                            BirthDate = new DateTime(1968, 7, 22),
                            RoleId = 4,
                            RegionId = 20,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Евгений",
                            Surname = "Чёрный",
                            BirthDate = new DateTime(2001, 2, 12),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Руслан",
                            Surname = "Шимчук",
                            BirthDate = new DateTime(2008, 12, 2),
                            RoleId = 4,
                            RegionId = 21,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Анастасия",
                            Surname = "Костюк",
                            BirthDate = new DateTime(2002, 2, 16),
                            RoleId = 4,
                            RegionId = 1,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Юлия",
                            Surname = "Азарова",
                            BirthDate = new DateTime(1999, 11, 6),
                            RoleId = 4,
                            RegionId = 15,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Анна",
                            Surname = "Марусиченко",
                            BirthDate = new DateTime(2004, 7, 24),
                            RoleId = 4,
                            RegionId = 26,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Дарья",
                            Surname = "Томаш",
                            BirthDate = new DateTime(2001, 5, 12),
                            RoleId = 4,
                            RegionId = 27,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Юлия",
                            Surname = "Бугаец",
                            BirthDate = new DateTime(1997, 2, 2),
                            RoleId = 4,
                            RegionId = 7,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Светлана",
                            Surname = "Пашиста",
                            BirthDate = new DateTime(2003, 8, 23),
                            RoleId = 4,
                            RegionId = 9,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Татьяна",
                            Surname = "Клепикова",
                            BirthDate = new DateTime(1995, 6, 20),
                            RoleId = 4,
                            RegionId = 4,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Татьяна",
                            Surname = "Олейник",
                            BirthDate = new DateTime(1994, 4, 10),
                            RoleId = 4,
                            RegionId = 22,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Александр",
                            Surname = "Авраменко",
                            BirthDate = new DateTime(1977, 7, 18),
                            RoleId = 4,
                            RegionId = 25,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Василий",
                            Surname = "Рублевский",
                            BirthDate = new DateTime(1999, 4, 8),
                            RoleId = 4,
                            RegionId = 14,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Карина",
                            Surname = "Бойсюк",
                            BirthDate = new DateTime(1978, 4, 12),
                            RoleId = 4,
                            RegionId = 24,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Анна",
                            Surname = "Волкова",
                            BirthDate = new DateTime(2002, 3, 2),
                            RoleId = 4,
                            RegionId = 12,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Полина",
                            Surname = "Волошина",
                            BirthDate = new DateTime(1972, 6, 12),
                            RoleId = 4,
                            RegionId = 4,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Дарья",
                            Surname = "Гладкая",
                            BirthDate = new DateTime(1997, 9, 22),
                            RoleId = 4,
                            RegionId = 20,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Марьяна",
                            Surname = "Демченко",
                            BirthDate = new DateTime(2003, 2, 1),
                            RoleId = 4,
                            RegionId = 7,
                            ClubId = 4
                        },
                        new User
                        {
                            Name = "Дарья",
                            Surname = "Додон",
                            BirthDate = new DateTime(2001, 12, 12),
                            RoleId = 4,
                            RegionId = 5,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Инна",
                            Surname = "Игнатьева",
                            BirthDate = new DateTime(1993, 2, 26),
                            RoleId = 4,
                            RegionId = 6,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Евгения",
                            Surname = "Магас",
                            BirthDate = new DateTime(2004, 8, 1),
                            RoleId = 4,
                            RegionId = 17,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Наталия",
                            Surname = "Малькова",
                            BirthDate = new DateTime(1984, 4, 17),
                            RoleId = 4,
                            RegionId = 19,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Анастасия",
                            Surname = "Мищук",
                            BirthDate = new DateTime(1994, 8, 27),
                            RoleId = 4,
                            RegionId = 9,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Тарас",
                            Surname = "Гаврилюк",
                            BirthDate = new DateTime(2001, 8, 7),
                            RoleId = 4,
                            RegionId = 19,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Максим",
                            Surname = "Дворный",
                            BirthDate = new DateTime(1956, 3, 27),
                            RoleId = 4,
                            RegionId = 23,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Михаил",
                            Surname = "Давиденко",
                            BirthDate = new DateTime(1986, 8, 7),
                            RoleId = 4,
                            RegionId = 26,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Богдан",
                            Surname = "Кабанов",
                            BirthDate = new DateTime(1985, 4, 27),
                            RoleId = 4,
                            RegionId = 14,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Денис",
                            Surname = "Кобзар",
                            BirthDate = new DateTime(1965, 4, 12),
                            RoleId = 4,
                            RegionId = 17,
                            ClubId = 1
                        },
                        new User
                        {
                            Name = "Владимир",
                            Surname = "Куцак",
                            BirthDate = new DateTime(2001, 7, 11),
                            RoleId = 4,
                            RegionId = 11,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Константин",
                            Surname = "Комарницкий",
                            BirthDate = new DateTime(1991, 6, 1),
                            RoleId = 4,
                            RegionId = 16,
                            ClubId = 6
                        },
                        new User
                        {
                            Name = "Юрий",
                            Surname = "Черняк",
                            BirthDate = new DateTime(2000, 4, 12),
                            RoleId = 4,
                            RegionId = 21,
                            ClubId = 7
                        },
                        new User
                        {
                            Name = "Андрей",
                            Surname = "Андрусяк",
                            BirthDate = new DateTime(1994, 10, 7),
                            RoleId = 4,
                            RegionId = 11,
                            ClubId = 5
                        },
                        new User
                        {
                            Name = "Иван",
                            Surname = "Беляев",
                            BirthDate = new DateTime(1965, 10, 5),
                            RoleId = 4,
                            RegionId = 8,
                            ClubId = 1
                        }

                        );
                }

                if (!context.Logins.Any())
                {
                    for (int i = 1; i <= context.Users.Count<User>(); i++)
                    {
                        string login = "sportsman" + i.ToString();
                        if (i == 30)
                            login = "admin";
                        else if (i == 31 || i == 32)
                            login = "moderator" + (i % 10).ToString();
                        else if (i >= 33 && i <= 36)
                            login = "organizer" + (i % 10).ToString();
                        int id = i;
                        string hashedPassword = HashPassword(login);
                        context.Logins.Add(new LoginData { Login = login, UserId = id, HashedPassword = hashedPassword });
                    }
                }

                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                        new Event
                        {
                            Title = "Старт, посвященный памяти Рикуна В. Б.",
                            EventDate = new DateTime(2020, 12, 6),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2",
                            Location = "с. Китайгород",
                            RegionId = 23,
                            OrganizerId = 30
                        },
                        new Event
                        {
                            Title = "Открытый турнир, посвященный Дню Энергетика",
                            EventDate = new DateTime(2020, 12, 12),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "с. Катеринка",
                            RegionId = 23,
                            OrganizerId = 31
                        },
                        new Event
                        {
                            Title = "Тренировочный старт \"Днепровские пороги\"",
                            EventDate = new DateTime(2020, 12, 13),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2",
                            Location = "с. Майорка (дачи)",
                            RegionId = 12,
                            OrganizerId = 32
                        },
                        new Event
                        {
                            Title = "Відкриті змагання Донецької області зі спортивного орієнтування (бігом), присвячені Дню Святого Миколая.",
                            EventDate = new DateTime(2020, 12, 19),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "Відокремлений підрозділ громадської організації ”Федерація спортивного орієнтування України” у Донецькій області.",
                            RegionId = 22,
                            OrganizerId = 33
                        },
                        new Event
                        {
                            Title = "Традиційні змагання зі спортивного орієнтування \"Сніжинка 2021\"",
                            EventDate = new DateTime(2020, 12, 19),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "КСОТ \"Центуріон\"",
                            RegionId = 10,
                            OrganizerId = 34
                        },
                        new Event
                        {
                            Title = "Закрытие сезона",
                            EventDate = new DateTime(2020, 12, 20),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2",
                            Location = "с/к \"Наш клуб\"",
                            RegionId = 23,
                            OrganizerId = 35
                        },
                        new Event
                        {
                            Title = "Ёлки-палки",
                            EventDate = new DateTime(2020, 12, 25),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2",
                            Location = "с/к \"Сириус\"",
                            RegionId = 23,
                            OrganizerId = 36
                        },
                        new Event
                        {
                            Title = "Різдвяний старт",
                            EventDate = new DateTime(2020, 12, 26),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "Відокремлений підрозділ громадської організації \"Федерація спортивного орієнтування України\" у Донецькій області.",
                            RegionId = 22,
                            OrganizerId = 31
                        },
                        new Event
                        {
                            Title = "НОВОРІЧНА КОРИДА 2020",
                            EventDate = new DateTime(2020, 12, 27),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2",
                            Location = "Лижна база \"Авангард\", ур.Токарі",
                            RegionId = 7,
                            OrganizerId = 32
                        },
                        new Event
                        {
                            Title = "Супер кубок приз мильярд денег",
                            EventDate = new DateTime(2020, 12, 31),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "дом Колотушкина",
                            RegionId = 24,
                            OrganizerId = 33
                        },
                        new Event
                        {
                            Title = "Марафон веб-дизайнеров",
                            EventDate = new DateTime(2020, 12, 30),
                            InfoLink = "",
                            ResultsLink = "",
                            Location = "с. Бутстрап",
                            RegionId = 3,
                            OrganizerId = 34
                        },
                        new Event
                        {
                            Title = "Биба Бобовна",
                            EventDate = new DateTime(2021, 1, 7),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3044&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3044&inf=2",
                            Location = "с. Моя хата",
                            RegionId = 17,
                            OrganizerId = 35
                        },
                        new Event
                        {
                            Title = "Кубок нероб",
                            EventDate = new DateTime(2021, 1, 14),
                            InfoLink = "http://orientsumy.com.ua/index.php?event=3086&inf=1",
                            ResultsLink = "http://orientsumy.com.ua/index.php?event=3086&inf=2",
                            Location = "с. Чилловое",
                            RegionId = 14,
                            OrganizerId = 36
                        }
                        ); ;
                }

                if (!context.Groups.Any())
                {
                    context.Groups.AddRange(
                        new Group { Name = "Ж12" },
                        new Group { Name = "Ж14" },
                        new Group { Name = "Ж16" },
                        new Group { Name = "Ж18" },
                        new Group { Name = "Ж20" },
                        new Group { Name = "Ж21Е" },
                        new Group { Name = "Ж21А" },
                        new Group { Name = "Ж35" },
                        new Group { Name = "Ж40" },
                        new Group { Name = "Ж45" },
                        new Group { Name = "Ж50" },
                        new Group { Name = "Ж55" },
                        new Group { Name = "Ж60" },
                        new Group { Name = "Ж65" },
                        new Group { Name = "Ж70" },
                        new Group { Name = "Ж75" },
                        new Group { Name = "М12" },
                        new Group { Name = "М14" },
                        new Group { Name = "М16" },
                        new Group { Name = "М18" },
                        new Group { Name = "М20" },
                        new Group { Name = "М21Е" },
                        new Group { Name = "М21А" },
                        new Group { Name = "М35" },
                        new Group { Name = "М40" },
                        new Group { Name = "М45" },
                        new Group { Name = "М50" },
                        new Group { Name = "М55" },
                        new Group { Name = "М60" },
                        new Group { Name = "М65" },
                        new Group { Name = "М70" },
                        new Group { Name = "М75" }
                        );
                }

                if (!context.EventGroups.Any())
                {
                    List<int> GroupIds_Template_1 = new List<int> { 1, 30, 25, 26, 24, 20, 15, 14, 9, 10, 8, 4 };
                    List<int> GroupIds_Template_2 = new List<int> { 30, 29, 28, 27, 26, 20, 11, 10, 9, 8, 7, 6 };
                    List<int> GroupIds_Template_3 = new List<int> { 1, 24, 23, 22, 21, 20, 13, 12, 11, 9, 5, 3 };
                    List<int> GroupIds_Template_4 = new List<int> { 1, 30, 29, 28, 27, 13, 12, 11, 8, 5, 4, 3 };

                    for (int i = 1; i <= 12; i += 4)
                    {
                        foreach (int groupId in GroupIds_Template_1)
                        {
                            context.EventGroups.Add(new EventGroup { GroupId = groupId, EventId = (i) });
                        }

                        foreach (int groupId in GroupIds_Template_2)
                        {
                            context.EventGroups.Add(new EventGroup { GroupId = groupId, EventId = (i + 1) });
                        }

                        foreach (int groupId in GroupIds_Template_3)
                        {
                            context.EventGroups.Add(new EventGroup { GroupId = groupId, EventId = (i + 2) });
                        }

                        foreach (int groupId in GroupIds_Template_4)
                        {
                            context.EventGroups.Add(new EventGroup { GroupId = groupId, EventId = (i + 3) });
                        }
                    }

                    foreach (int groupId in GroupIds_Template_1)
                    {
                        context.EventGroups.Add(new EventGroup { GroupId = groupId, EventId = 13 });
                    }

                }

                if (!context.Applications.Any())
                {
                    Dictionary<int, List<int>> application = new Dictionary<int, List<int>>();

                    for (int i = 1; i <= context.Users.Count<User>(); i++)
                    {
                        application.Add(i, new List<int>());
                    }

                    Random rand = new Random();

                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < 30; j++)
                        {
                            int eventGroupId = rand.Next(1, 157);
                            int chipId = 0;
                            if (rand.Next(0, 101) > 50)
                                chipId = rand.Next(1000000, 10000000);
                            int userId = rand.Next(1, 58);
                            if (!application[userId].Contains(eventGroupId))
                            {
                                context.Applications.Add(new Application { EventGroupId = eventGroupId, ChipId = chipId, UserId = userId });
                                application[userId].Add(eventGroupId);
                            }

                        }
                    }
                }

                context.SaveChanges();
            }
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

    }
}
