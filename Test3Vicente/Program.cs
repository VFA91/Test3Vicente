using Autofac;
using Data;
using Data.Repositories;
using Domain.Core;
using Domain.Core.Respositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test3Vicente;

namespace Test3Vicente
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = Autofac();

            using (var scope = builder.BeginLifetimeScope())
            {
                var init = scope.Resolve<IApp>();
                init.Run();
            }

            //LoadDataBase();


            //GetNameGuestOneEvent();
            //UpdateDateEvent(7);
        }

        

        private static IContainer Autofac()
        {
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            // Register types that expose interfaces...
            builder.RegisterType<DataFactory>().As<IDataFactory>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<CityRepository>().As<ICityRepository>().SingleInstance();
            builder.RegisterType<GuestRepository>().As<IGuestRepository>().SingleInstance();
            builder.RegisterType<EventRepository>().As<IEventRepository>().SingleInstance();
            builder.RegisterType<ServiceRepository>().As<IServiceRepository>().SingleInstance();
            builder.RegisterType<App>().As<IApp>().SingleInstance();

            // Build the container to finalize registrations
            // and prepare for object resolution.
            return builder.Build();
        }
    }
}
