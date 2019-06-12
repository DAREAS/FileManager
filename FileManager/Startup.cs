using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FileManager.Infrastructure.ContractsResolver;
using FileManager.Repository.Infrastructure;
using FileManager.WebApi.Container;
using FileManager.WebApi.Jobs;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace FileManager.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options => options.UseInMemoryDatabase(databaseName: "FileManager"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureHangfire(services);

            services.AddLogging(logging => logging.AddSeq());

            services.AddSwaggerGen(sw =>
            {
                sw.SwaggerDoc("v1", new Info
                {
                    Title = "Gerenciador de Arquivos",
                    Version = "v1",
                    Description = "Api para gerenciamento de arquivos de vídeo",
                    Contact = new Contact
                    {
                        Name = "Diogo Arêas",
                        Email = "dfcareas@gmail.com"
                    }
                });

                string basePath = AppContext.BaseDirectory;
                string assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                string fileName = System.IO.Path.GetFileName(assemblyName + ".xml");

                sw.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));
            });

            var serviceProvider = ConfigureServiceProvider(services);

            return serviceProvider;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="fileManagerSchedule"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, FileManagerSchedule fileManagerSchedule)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddSeq(Configuration.GetSection("seq"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(sw =>
            {
                sw.SwaggerEndpoint("/swagger/v1/swagger.json", "Gerenciador de Arquivos");
            });

            fileManagerSchedule.StartBckgroundJob();
        }

        private IServiceProvider ConfigureServiceProvider(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new MainModule() { Configuration = this.Configuration });

            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            return serviceProvider;
        }

        private static void ConfigureHangfire(IServiceCollection services)
        {

            //services.AddHangfire(configuration => configuration.UseSqlServerStorage("Data Source=.\\sql2016;Initial Catalog=FileManager;Integrated Security=True;Trusted_Connection=True;"));
            services.AddHangfire(configuration => configuration.UseMemoryStorage());

            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(services.BuildServiceProvider()));

            services.AddTransient<FileManagerSchedule>();
        }

        private static void SetGlobalJsonSerializerSettings()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new IgnoreEmptyEnumerablesResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };
        }
    }
}
