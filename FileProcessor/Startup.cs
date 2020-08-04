using AutoMapper;
using Bll.Automapper;
using Bll.Helpers;
using Bll.Parsers;
using Dal.Repositories;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
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

namespace FileProcessor
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
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
            services.AddScoped<IErrorMessageHelper, ErrorMessageHelper>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
