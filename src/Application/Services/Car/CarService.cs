using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Car
{
    public class CarService : ICarService
    {
        ICarRepository _carRepository;
        IPhotoRepository _photoRepository;

        public CarService(ICarRepository carRepository, IPhotoRepository photoRepository)
        {
            _carRepository = carRepository;
            _photoRepository = photoRepository;
        }
        public async Task<Domain.Entities.Car> AddCar(CarDTO carDTO)
        {
            var car = await _carRepository.AddAsync(carDTO.Name, carDTO.Base64);
            return car;
        }

        public async Task DeleteCar(int id)
        {
            var car = await _carRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found");
            var rowsAffected = await _carRepository.DeleteAsync(id);
            if (rowsAffected == 0)
            {
                throw new Exception("Error deleting car");
            }
        }

        public async Task<CarModel> GetCar(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                throw new NotFoundException($"Car with ID {id} not found");
            }
            return new CarModel(id, car.Name, car.Photo.Base64);
        }

        public async Task<List<CarModel>> GetCars(string query, int page)
        {
            List<CarModel> cars = new List<CarModel>();

            var dbCars = await _carRepository.GetAllAsync(query, page);
            foreach (var car in dbCars)
            {
                cars.Add(new CarModel(car.Id, car.Name, car.Photo.Base64));
            }

            return cars;
        }

        public async Task UpdateCar(int id, CarDTO carDTO)
        {
            if (string.IsNullOrEmpty(carDTO.Base64) && string.IsNullOrEmpty(carDTO.Name))
            {
                throw new Exception("Name or File must be provided");
            }

            Domain.Entities.Car car = await _carRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Car with ID {id} not found");

            if (!string.IsNullOrEmpty(carDTO.Base64))
            {
                car.Photo.Base64 = carDTO.Base64;
            }

            if (!string.IsNullOrEmpty(carDTO.Name))
            {
                car.Name = carDTO.Name;
            }

            await _carRepository.UpdateAsync(car);

        }
    }
}
