using System;
using System.IO;
using AutoMapper;
using EMenuApplication.Models;
using EMenuApplication.Repository;
using EMenuApplication.Repository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EMenuApplication
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
            // Add EF services to the services container.
            services.AddDbContext<EMenuDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Add other services
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IItemTagRepository, ItemTagRepository>();
            services.AddTransient<IMenuItemRepository, MenuItemRepository>();
            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<IMenuScheduleRepository, MenuScheduleRepository>();
            services.AddTransient<IConceptsRepository, ConceptsRepository>();
            services.AddTransient<IStoresRespository, StoresRespository>();
            services.AddTransient<ISurveyRepository, SurveyRepository>();
            services.AddTransient<IDashboardRepository, DashboardRepository>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IConceptThemeRepository, ConceptThemeRepository>();
            services.AddTransient<IRegionRepository, RegionRepository>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IVoucherSetupRepository, VoucherSetupRepository>();
            services.AddTransient<IVoucherReasonCategoryMasterRepository, VoucherReasonCategoryMasterRepository>();
            services.AddTransient<IVoucherReasonSubCategoryMasterRepository, VoucherReasonSubCategoryMasterRepository>();
            services.AddTransient<IVoucherIssuanceRepository, VoucherIssuanceRepository>();
            services.AddTransient<ICustomersRespository, CustomersRespository>();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddAutoMapper(typeof(Startup));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, Configuration.GetValue<string>("UploadFolder"))),
                RequestPath = "/Image"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, Configuration.GetValue<string>("UploadFolder"))),
                RequestPath = "/Image"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, Configuration.GetValue<string>("UploadFolder"))),
                RequestPath = "/Image"
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");


            app.UseSession();
            AppContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            loggerFactory.AddFile("Logs/myapp-{Date}.txt");
        }
    }
}
