using Application.DTOs;
using Application.Models;
using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Car
{
    public class CarService : ICarService
    {
        ICarRepository _carRepository;

        public CarService(ICarRepository carRepository, IPhotoRepository photoRepository)
        {
            _carRepository = carRepository;
        }
        public async Task<Domain.Entities.Car> AddCar(CarDTO carDTO)
        {
            if (string.IsNullOrEmpty(carDTO.Base64) || string.IsNullOrEmpty(carDTO.Name))
            {
                throw new Exception("Name and File must be provided");
            }
            var car = await _carRepository.AddAsync(carDTO.Name, carDTO.Base64) ?? throw new Exception("Error adding car");
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

        public async Task<PaginationModel<CarModel>> GetCars(string? query, int page)
        {
            List<CarModel> cars = new List<CarModel>();

            var tupleCars = await _carRepository.GetAllAsync(query, page);
            var dbCars = tupleCars.Item1;
            foreach (var car in dbCars)
            {
                cars.Add(new CarModel(car.Id, car.Name, car.Photo.Base64));
            }
            int totalItems = tupleCars.Item2;
            int totalPages = tupleCars.Item3;

            return new PaginationModel<CarModel> { Items = cars, TotalItems = totalItems, TotalPages = totalPages };
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
