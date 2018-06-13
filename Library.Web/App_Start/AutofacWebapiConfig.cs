using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Library.Data;
using Library.Data.Infastructure;
using Library.Service;

namespace Library.Web.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<LibraryEntities>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReserveRepository>()
                .As<IReserveRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AuthorRepository>()
                .As<IAuthorRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BooksRepository>()
                .As<IBooksRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<LogRepository>().As<ILogRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AuthorService>()
                .As<IAuthorService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BookService>()
                .As<IBookService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ReserveService>()
                .As<IReserveService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

            
            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}