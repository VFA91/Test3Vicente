using Domain.Core;
using Domain.Core.Respositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3Vicente
{
    public class App : IApp
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public App(ICityRepository cityRepository, IUnitOfWork unitOfWork)
        {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
        }

        public void Run()
        {
            InsertCity();
        }

        private void InsertCity()
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
