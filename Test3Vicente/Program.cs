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

        //private static List<string> GetNameGuestOneEvent()
        //{
        //    List<string> names = new List<string>();
        //    //Escriba un método en que liste el nombre de todos los invitados que están inscriptos en al menos un evento
        //    using (var db = new EventManagement())
        //    {
        //        var guests = db.Guest.Include("Event").Where(g => g.Event.Any());
        //        names = guests.Select(n => n.FirstName).ToList();
        //    }

        //    return names;
        //}

        //private static void UpdateDateEvent(int days)
        //{
        //    //Escriba un método en que actualice la fecha de todos los eventos programados para que sean realizados una semana después.
        //    using (var db = new EventManagement())
        //    {
        //        var result = db.Event.Where(e => e.EventId > 0);

        //        foreach (var item in result)
        //        {
        //            item.Date = item.Date.AddDays(7);
        //        }
        //        db.SaveChanges();
        //    }
        //}

        private static IContainer Autofac()
        {
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            // Register types that expose interfaces...
            builder.RegisterType<DataFactory>().As<IDataFactory>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            builder.RegisterType<CityRepository>().As<ICityRepository>().SingleInstance();
            builder.RegisterType<App>().As<IApp>().SingleInstance();

            // Build the container to finalize registrations
            // and prepare for object resolution.
            return builder.Build();
        }
    }
}
