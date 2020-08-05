using AutoMapper;
using Bll.Automapper;
using Bll.Helpers;
using Bll.Managers;
using Bll.Parsers;
using Bll.Validators;
using Dal.Repositories;
using Dal.Services;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Validators;
using Domain.Models.CSV;
using Domain.Models.Validation;
using EfContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FileProcessor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var conStr = Configuration.GetConnectionString("TransactionsDb");

            services.AddDbContext<TransactionContext>(options =>
            {
                options.UseSqlServer(conStr);
            });

            services.AddScoped<IFileParserFactory, FileParserFactory>();
            services.AddScoped<IMessageBuilder, MessageBuilder>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionValidator, TransactionValidator>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileManager, FileManager>();

            services.AddScoped<CsvParser>();
            services.AddScoped<XmlParser>();
            services.AddScoped<MessageBuilder>();

            var csvOptions = Configuration.GetSection("CsvOptions");
            services.Configure<CsvOptions>(csvOptions);

            var validationRules = Configuration.GetSection("ValidationRules");
            services.Configure<ValidationRules>(validationRules);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TransactionProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
