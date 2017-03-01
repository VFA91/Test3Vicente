using Domain.Core;
using Domain.Core.Respositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3Vicente
{
    public class App : IApp
    {
        private const int WEEK = 7;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICityRepository _cityRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IServiceRepository _serviceRepository;

        public App(IUnitOfWork unitOfWork,
            ICityRepository cityRepository,
            IEventRepository eventRepository,
            IGuestRepository guestRepository,
            IServiceRepository serviceRepository)
        {
            _unitOfWork = unitOfWork;
            _cityRepository = cityRepository;
            _eventRepository = eventRepository;
            _guestRepository = guestRepository;
            _serviceRepository = serviceRepository;
        }

        public void Run()
        {
            InserDataToDataBase();
            GetNameGuestOneEvent();
            UpdateDateEvent(WEEK);
        }

        /// <summary>
        /// Escriba un método en que liste el nombre de todos los invitados que están inscriptos en al menos un evento
        /// </summary>
        /// <returns></returns>
        private List<string> GetNameGuestOneEvent()
        {
            var guests = _guestRepository.GetAll().Include("Event").Where(g => g.Event.Any());
            var names = guests.Select(n => n.FirstName).ToList();

            return names;
        }

        /// <summary>
        /// Escriba un método en que actualice la fecha de todos los eventos programados para que sean realizados una semana después.
        /// </summary>
        /// <param name="days"></param>
        private void UpdateDateEvent(int days)
        {
            var events = _eventRepository.GetAll();

            foreach (var item in events.ToList())
            {
                item.Date = item.Date.AddDays(7);
                _eventRepository.Update(item);
            }

            _unitOfWork.Commit();
        }

        private void InserDataToDataBase()
        {
            bool insertRelationship = false;
            InsertCity();
            insertRelationship = InsertGuest();
            insertRelationship = InsertEvent();
            insertRelationship = InsertService();

            if (insertRelationship)
            {
                InsertGuestEvent();
                InsertGuestService();
                InsertEventService();
            }
        }

        private void InsertEventService()
        {
            var eventIds = _eventRepository.GetAll().ToList().Select(e => e.EventId);
            var serviceIds = _serviceRepository.GetAll().ToList().Select(s => s.ServiceId);

            for (int i = 0; i < 15; i++)
            {
                int eventId, serviceId;
                RandomIds(eventIds, serviceIds, out eventId, out serviceId);

                var eventEntity = _eventRepository.GetById(eventId);
                eventEntity.Service.Add(_serviceRepository.GetById(serviceId));
                _unitOfWork.Commit();
            }
        }

        private void InsertGuestEvent()
        {
            var guestIds = _guestRepository.GetAll().ToList().Select(g => g.GuestId);
            var eventIds = _eventRepository.GetAll().ToList().Select(e => e.EventId);

            for (int i = 0; i < 15; i++)
            {
                int guestId, eventId;
                RandomIds(guestIds, eventIds, out guestId, out eventId);

                var guest = _guestRepository.GetById(guestId);
                guest.Event.Add(_eventRepository.GetById(eventId));
                _unitOfWork.Commit();
            }
        }

        private static void RandomIds(IEnumerable<int> firstIds, IEnumerable<int> secondsIds, out int firstId, out int secondId)
        {
            Random rnd = new Random();
            firstId = rnd.Next(firstIds.Count());
            secondId = rnd.Next(secondsIds.Count());
            firstId = firstId == 0 ? firstId + 1 : firstId;
            secondId = secondId == 0 ? secondId + 1 : secondId;
        }

        private void InsertGuestService()
        {
            var guestIds = _guestRepository.GetAll().ToList().Select(g => g.GuestId);
            var serviceIds = _serviceRepository.GetAll().ToList().Select(s => s.ServiceId);

            for (int i = 0; i < 15; i++)
            {
                int guestId, serviceId;
                RandomIds(guestIds, serviceIds, out guestId, out serviceId);

                var guest = _guestRepository.GetById(guestId);
                guest.Service.Add(_serviceRepository.GetById(serviceId));
                _unitOfWork.Commit();
            }
        }

        private bool InsertService()
        {
            var numServices = _serviceRepository.GetAll().Count();
            if (numServices <= 0)
            {
                _serviceRepository.Add(new Service { Name = "Servicio 1", CUP = 65, PVP = 100 });
                _serviceRepository.Add(new Service { Name = "Servicio 2", CUP = 777, PVP = 1000 });
                _serviceRepository.Add(new Service { Name = "Servicio 3", CUP = 80, PVP = 120 });
                _serviceRepository.Add(new Service { Name = "Servicio 4", CUP = 135, PVP = 260 });
                _serviceRepository.Add(new Service { Name = "Servicio 5", CUP = 20, PVP = 30 });
                _serviceRepository.Add(new Service { Name = "Servicio 6", CUP = 1000, PVP = 2500 });
                _serviceRepository.Add(new Service { Name = "Servicio 7", CUP = 300, PVP = 350 });
                _serviceRepository.Add(new Service { Name = "Servicio 8", CUP = 200, PVP = 400 });

                _unitOfWork.Commit();

                return true;
            }

            return false;
        }

        private bool InsertEvent()
        {
            var numEvents = _eventRepository.GetAll().Count();
            if (numEvents <= 0)
            {
                _eventRepository.Add(new Event { Description = "Evento 1", Date = DateTime.Now.AddDays(1), CityId = 1 });
                _eventRepository.Add(new Event { Description = "Evento 2", Date = DateTime.Now.AddDays(10), CityId = 1 });
                _eventRepository.Add(new Event { Description = "Evento 3", Date = DateTime.Now.AddDays(20), CityId = 1 });
                _eventRepository.Add(new Event { Description = "Evento 4", Date = DateTime.Now.AddDays(30), CityId = 2 });
                _eventRepository.Add(new Event { Description = "Evento 5", Date = DateTime.Now.AddDays(40), CityId = 2 });
                _eventRepository.Add(new Event { Description = "Evento 6", Date = DateTime.Now.AddDays(50), CityId = 3 });
                _eventRepository.Add(new Event { Description = "Evento 7", Date = DateTime.Now.AddDays(60), CityId = 3 });
                _eventRepository.Add(new Event { Description = "Evento 8", Date = DateTime.Now.AddDays(70), CityId = 4 });
                _eventRepository.Add(new Event { Description = "Evento 9", Date = DateTime.Now.AddDays(80), CityId = 5 });
                _eventRepository.Add(new Event { Description = "Evento 10", Date = DateTime.Now.AddDays(90), CityId = 6 });

                _unitOfWork.Commit();

                return true;
            }

            return false;
        }

        private bool InsertGuest()
        {
            var numGuests = _guestRepository.GetAll().Count();

            if (numGuests <= 0)
            {
                _guestRepository.Add(new Guest { FirstName = "Vicente", LastName = "Fernández", Mail = "Vicente@gmail.com", CityId = 2 });
                _guestRepository.Add(new Guest { FirstName = "Paco", LastName = "Hernández", Mail = "Paco@gmail.com", CityId = 1, Phone = 999999999 });
                _guestRepository.Add(new Guest { FirstName = "Luis", LastName = "García", Mail = "Luis@gmail.com", CityId = 3 });
                _guestRepository.Add(new Guest { FirstName = "Maria", LastName = "López", Mail = "Maria@gmail.com", CityId = 5 });
                _guestRepository.Add(new Guest { FirstName = "Antonia", LastName = "Horcajada", Mail = "Antonia@gmail.com", CityId = 1 });
                _guestRepository.Add(new Guest { FirstName = "Lucio", LastName = "Antolín", Mail = "Lucio@gmail.com", CityId = 3, Phone = 777777777 });
                _guestRepository.Add(new Guest { FirstName = "Carlos", LastName = "Caminero", Mail = "Carlos@gmail.com", CityId = 1 });
                _guestRepository.Add(new Guest { FirstName = "Antonio", LastName = "Rojo", Mail = "Antonio@gmail.com", CityId = 9 });
                _guestRepository.Add(new Guest { FirstName = "Mar", LastName = "López", Mail = "Mar@gmail.com", CityId = 1 });
                _guestRepository.Add(new Guest { FirstName = "Juan", LastName = "Hoz", Mail = "Juan@gmail.com", CityId = 11 });
                _guestRepository.Add(new Guest { FirstName = "Hector", LastName = "García", Mail = "Hector@gmail.com", CityId = 3, Phone = 666666666 });
                _guestRepository.Add(new Guest { FirstName = "Jon", LastName = "Hernández", Mail = "Jon@gmail.com", CityId = 7 });
                _guestRepository.Add(new Guest { FirstName = "Laura", LastName = "Hoz", Mail = "Laura@gmail.com", CityId = 1 });
                _guestRepository.Add(new Guest { FirstName = "Jose", LastName = "Horcajada", Mail = "Jose@gmail.com", CityId = 3 });
                _guestRepository.Add(new Guest { FirstName = "Esperanza", LastName = "Fernández", Mail = "Esperanza@gmail.com", CityId = 1 });
                _guestRepository.Add(new Guest { FirstName = "Julio", LastName = "Caminero", Mail = "Julio@gmail.com", CityId = 10 });

                _unitOfWork.Commit();

                return true;
            }

            return false;
        }

        private void InsertCity()
        {
            var numCities = _cityRepository.GetAll().Count();

            if (numCities <= 0)
            {
                _cityRepository.Add(new City { Name = "Madrid" });
                _cityRepository.Add(new City { Name = "Palencia" });
                _cityRepository.Add(new City { Name = "Barcelona" });
                _cityRepository.Add(new City { Name = "Valladolid" });
                _cityRepository.Add(new City { Name = "Burgos" });
                _cityRepository.Add(new City { Name = "Valencia" });
                _cityRepository.Add(new City { Name = "Sevilla" });
                _cityRepository.Add(new City { Name = "Leon" });
                _cityRepository.Add(new City { Name = "Santander" });
                _cityRepository.Add(new City { Name = "Zaragoza" });
                _cityRepository.Add(new City { Name = "San Sebastián" });

                _unitOfWork.Commit();
            }
        }
    }
}
