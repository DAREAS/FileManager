using Autofac;
using System.Reflection;
using FileManager.Core.Repositories;
using FileManager.Repository.Infrastructure;
using AutoMapper;
using FileManager.Core.Operations;
using System.Collections.Generic;
using FileManager.Core.Mappers.Profiles;

namespace FileManager.WebApi.Container
{
    /// <summary>
    /// 
    /// </summary>
    public class MainModule : Autofac.Module
    {
        /// <summary>
        /// 
        /// </summary>
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var assemblies = new[]
            {
                typeof(Startup).GetTypeInfo().Assembly,
                typeof(IFileRepository).GetTypeInfo().Assembly,
                typeof(ApiDbContext).GetTypeInfo().Assembly,
                typeof(IOperation<>).GetTypeInfo().Assembly
            };

            containerBuilder.RegisterAssemblyTypes(assemblies)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.Register(ctx =>
            {
                var profiles = ctx.Resolve<IEnumerable<Profile>>();

                var config = new MapperConfiguration(cfg =>
                {
                    foreach(var profile in profiles)
                    {
                        cfg.AddProfile(profile);
                    }

                    cfg.AddProfile(new FileMapperProfile());
                });

                return config;
            })
            .SingleInstance() 
            .AutoActivate() 
            .AsSelf();

            containerBuilder.Register(tempContext =>
            {
                var ctx = tempContext.Resolve<IComponentContext>();
                var config = ctx.Resolve<MapperConfiguration>();

                return config.CreateMapper(t => ctx.Resolve(t));

            }).As<IMapper>();
        }
    }
}
