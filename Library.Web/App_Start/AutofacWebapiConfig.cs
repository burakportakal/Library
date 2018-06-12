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
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<LibraryEntities>()
                .As<DbContext>()
                .InstancePerRequest();

            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest();

            builder.RegisterType<ReserveRepository>()
                .As<IReserveRepository>()
                .InstancePerRequest();
            builder.RegisterType<AuthorRepository>()
                .As<IAuthorRepository>()
                .InstancePerRequest();
            builder.RegisterType<BooksRepository>()
                .As<IBooksRepository>()
                .InstancePerRequest();

            builder.RegisterType<AuthorService>()
                .As<IAuthorService>()
                .InstancePerRequest();
            builder.RegisterType<BookService>()
                .As<IBookService>()
                .InstancePerRequest();
            builder.RegisterType<ReserveService>()
                .As<IReserveService>()
                .InstancePerRequest();

            
            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}