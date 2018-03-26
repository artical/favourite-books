using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyFavouriteBooks.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.SpaServices.Webpack;
using MyFavouriteBooks.Models.Account;
using MyFavouriteBooks.Services.FileLogger;
using System.IO;
using MyFavouriteBooks.Services;
using Microsoft.Extensions.PlatformAbstractions;

namespace MyFavouriteBooks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserBooksRepository, EFUserBooksRepository>();
            services.AddTransient<IBookRepository, EFBookRepository>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"])
                );
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = false,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = false,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"])
                );
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "My Favourite Books App",
                    Version = "v1",
                    Contact = new Contact { Name = "Konstantin Ivanov", Email = "konstantin.ivn@gmail.com"},
                    Description = "ASP.NET Core Web API for user list of favourite books. It includes user registration and login, adding and removing favourite books from list."
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Name = "Authorization",
                        In = "header"
                    }
                );
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyFavouriteBooks.xml");
                c.IncludeXmlComments(filePath);
            });

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseAuthentication();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Favourite Books API V1");

            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
