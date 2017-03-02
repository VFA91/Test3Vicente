using Domain.Core;
using Domain.Core.Respositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            if (unitOfWork == null)
                throw new ArgumentNullException();
            _unitOfWork = unitOfWork;

            if (cityRepository == null)
                throw new ArgumentNullException();
            _cityRepository = cityRepository;

            if (eventRepository == null)
                throw new ArgumentNullException();
            _eventRepository = eventRepository;

            if (guestRepository == null)
                throw new ArgumentNullException();
            _guestRepository = guestRepository;

            if (serviceRepository == null)
                throw new ArgumentNullException();
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
            bool insertGuest = false;
            bool insertEvent = false;
            bool insertService = false;
            InsertCity();
            insertGuest = InsertGuest();
            insertEvent = InsertEvent();
            insertService = InsertService();

            InsertRelationship(insertGuest, insertEvent, insertService);
        }

        private void InsertRelationship(bool insertGuest, bool insertEvent, bool insertService)
        {
            if (insertGuest && insertEvent)
                InsertGuestEvent();

            if (insertService && insertEvent)
                InsertEventService();

            if (insertGuest && insertService)
                InsertGuestService();
        }

        #region INSERT Relationship Entities

        #region GuestEvent

        private void InsertGuestEvent()
        {
            var guestIds = _guestRepository.GetAll().ToList().Select(g => g.GuestId);
            var eventIds = _eventRepository.GetAll().ToList().Select(e => e.EventId);

            GenerateRelationshipGuestEvent(guestIds, eventIds);
        }

        private void GenerateRelationshipGuestEvent(IEnumerable<int> guestIds, IEnumerable<int> eventIds)
        {
            var numGuest = guestIds.Count();
            var numEvent = eventIds.Count();

            for (int i = 0; i < GetSeventyFivePercentId(numGuest); i++)
            {
                var guestId = guestIds.ElementAt(i);
                var guest = _guestRepository.GetById(guestId);
                InsertEventsInGuestIdentity(eventIds, numEvent, guest);
                DeleteRandomEventsInGuestIdentity(guest, eventIds);
            }

            _unitOfWork.Commit();
        }

        private void InsertEventsInGuestIdentity(IEnumerable<int> eventIds, int numEvent, Guest guest)
        {
            for (int i = 0; i < GetFiftyPercentId(numEvent); i++)
            {
                var eventId = eventIds.ElementAt(i);
                var eventEntity = _eventRepository.GetById(eventId);
                guest.Event.Add(eventEntity);
            }
        }

        private void DeleteRandomEventsInGuestIdentity(Guest guest, IEnumerable<int> eventIds)
        {
            var countEvent = guest.Event.Count();
            Random random = new Random();
            var number = random.Next(0, countEvent);
            for (int i = 0; i < number; i++)
            {
                var eventId = eventIds.ElementAt(i);
                var eventEntity = _eventRepository.GetById(eventId);
                guest.Event.Remove(eventEntity);
            }
        }

        #endregion

        #region EventService

        private void InsertEventService()
        {
            var eventIds = _eventRepository.GetAll().ToList().Select(e => e.EventId);
            var serviceIds = _serviceRepository.GetAll().ToList().Select(s => s.ServiceId);

            GenerateRelationshipEventService(eventIds, serviceIds);
        }

        private void GenerateRelationshipEventService(IEnumerable<int> eventIds, IEnumerable<int> serviceIds)
        {
            var numEvent = eventIds.Count();
            var numService = serviceIds.Count();

            for (int i = 0; i < GetSeventyFivePercentId(numEvent); i++)
            {
                var eventId = eventIds.ElementAt(i);
                var eventEntity = _eventRepository.GetById(eventId);
                InsertServicesInEventIdentity(serviceIds, numService, eventEntity);
                DeleteRandomServicesInEventIdentity(eventEntity, serviceIds);
            }

            _unitOfWork.Commit();
        }

        private void InsertServicesInEventIdentity(IEnumerable<int> serviceIds, int numService, Event eventEntity)
        {
            for (int i = 0; i < GetFiftyPercentId(numService); i++)
            {
                var serviceId = serviceIds.ElementAt(i);
                var serviceEntity = _serviceRepository.GetById(serviceId);
                eventEntity.Service.Add(serviceEntity);
            }
        }

        private void DeleteRandomServicesInEventIdentity(Event eventEntity, IEnumerable<int> serviceIds)
        {
            var countService = eventEntity.Service.Count();
            Random random = new Random();
            var number = random.Next(0, countService);
            for (int i = 0; i < number; i++)
            {
                var serviceId = serviceIds.ElementAt(i);
                var service = _serviceRepository.GetById(serviceId);
                eventEntity.Service.Remove(service);
            }
        }

        #endregion

        #region GuestService

        private void InsertGuestService()
        {
            var guestIds = _guestRepository.GetAll().Where(g => g.Event.Any()).ToList().Select(g => g.GuestId);
            var serviceIds = _serviceRepository.GetAll().Where(s => s.Event.Any()).ToList().Select(s => s.ServiceId);

            GenerateRelationshipGuestService(guestIds, serviceIds);
        }

        private void GenerateRelationshipGuestService(IEnumerable<int> guestIds, IEnumerable<int> serviceIds)
        {
            var numGuest = guestIds.Count();
            var numService = serviceIds.Count();

            for (int i = 0; i < GetSeventyFivePercentId(numGuest); i++)
            {
                var guestId = guestIds.ElementAt(i);
                var guest = _guestRepository.GetById(guestId);
                InsertServiceInGuestIdentity(serviceIds, numService, guest);
                DeleteRandomServiceInGuestIdentity(guest, serviceIds);
            }

            _unitOfWork.Commit();
        }

        private void InsertServiceInGuestIdentity(IEnumerable<int> serviceIds, int numService, Guest guest)
        {
            for (int i = 0; i < GetFiftyPercentId(numService); i++)
            {
                var servicetId = serviceIds.ElementAt(i);
                var service = _serviceRepository.GetById(servicetId);
                guest.Service.Add(service);
            }
        }

        private void DeleteRandomServiceInGuestIdentity(Guest guest, IEnumerable<int> serviceIds)
        {
            var countService = guest.Service.Count();
            Random random = new Random();
            var number = random.Next(0, countService);
            for (int i = 0; i < number; i++)
            {
                var serviceId = serviceIds.ElementAt(i);
                var service = _serviceRepository.GetById(serviceId);
                guest.Service.Remove(service);
            }
        }

        #endregion

        private int GetSeventyFivePercentId(int total)
        {
            return (total / 2) + (total / 2) / 2;
        }

        private int GetFiftyPercentId(int total)
        {
            return total / 2;
        }

        #endregion

        #region INSERT Entities

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
                _eventRepository.Add(new Event { Description = "Evento 1", Date = DateTime.Now.AddDays(1), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 2", Date = DateTime.Now.AddDays(10), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 3", Date = DateTime.Now.AddDays(20), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 4", Date = DateTime.Now.AddDays(30), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 5", Date = DateTime.Now.AddDays(40), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 6", Date = DateTime.Now.AddDays(50), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 7", Date = DateTime.Now.AddDays(60), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 8", Date = DateTime.Now.AddDays(70), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 9", Date = DateTime.Now.AddDays(80), CityId = GetCityId() });
                _eventRepository.Add(new Event { Description = "Evento 10", Date = DateTime.Now.AddDays(90), CityId = GetCityId() });

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
                _guestRepository.Add(new Guest { FirstName = "Vicente", LastName = "Fernández", Mail = "Vicente@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Paco", LastName = "Hernández", Mail = "Paco@gmail.com", CityId = GetCityId(), Phone = 999999999 });
                _guestRepository.Add(new Guest { FirstName = "Luis", LastName = "García", Mail = "Luis@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Maria", LastName = "López", Mail = "Maria@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Antonia", LastName = "Gutierrez", Mail = "Antonia@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Lucio", LastName = "Antolín", Mail = "Lucio@gmail.com", CityId = GetCityId(), Phone = 777777777 });
                _guestRepository.Add(new Guest { FirstName = "Carlos", LastName = "Caminero", Mail = "Carlos@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Antonio", LastName = "Rojo", Mail = "Antonio@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Mar", LastName = "López", Mail = "Mar@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Juan", LastName = "Hoz", Mail = "Juan@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Hector", LastName = "García", Mail = "Hector@gmail.com", CityId = GetCityId(), Phone = 666666666 });
                _guestRepository.Add(new Guest { FirstName = "Jon", LastName = "Hernández", Mail = "Jon@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Laura", LastName = "Hoz", Mail = "Laura@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Jose", LastName = "Gutierrez", Mail = "Jose@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Esperanza", LastName = "Fernández", Mail = "Esperanza@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Julio", LastName = "Caminero", Mail = "Julio@gmail.com", CityId = GetCityId() });
                _guestRepository.Add(new Guest { FirstName = "Ruben", LastName = "Ortefa", Mail = "Ruben@gmail.com", CityId = GetCityId(), Phone = 111111111 });
                _guestRepository.Add(new Guest { FirstName = "Miriam", LastName = "Ruiz", Mail = "Miriam@gmail.com", CityId = GetCityId(), Phone = 222222222 });
                _guestRepository.Add(new Guest { FirstName = "Eva", LastName = "María", Mail = "Eva@gmail.com", CityId = GetCityId(), Phone = 333333333 });
                _guestRepository.Add(new Guest { FirstName = "Elena", LastName = "Ayala", Mail = "Elena@gmail.com", CityId = GetCityId(), Phone = 444444444 });
                _guestRepository.Add(new Guest { FirstName = "Victor", LastName = "Fernández", Mail = "Victor@gmail.com", CityId = GetCityId(), Phone = 555555555 });
                _guestRepository.Add(new Guest { FirstName = "Jorge", LastName = "Martín", Mail = "Jorge@gmail.com", CityId = GetCityId(), Phone = 888888888 });

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

        private int GetCityId()
        {
            var cityIds = _cityRepository.GetAll();
            var ids = _cityRepository.GetAll().Select(i => i.CityId).ToList();
            var idRandom = GetIdRandom(ids);
            return cityIds.ToList().ElementAt(idRandom).CityId;
        }

        private static int GetIdRandom(List<int> ids)
        {
            Random random = new Random();
            var id = random.Next(ids.Count());
            id = id == 0 ? id + 1 : id;
            return id;
        }

        #endregion
    }
}
