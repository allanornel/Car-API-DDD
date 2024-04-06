using Application.DTOs;
using Application.Models;
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
        public async Task<CarResult> AddCar(CarDTO carDTO)
        {
            return new CarResult(1, carDTO.Name, carDTO.Status);
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
            List<CarModel> cars = new();

            var dbCars = await _carRepository.GetAllAsync(query, page);
            foreach (var car in dbCars)
            {
                cars.Add(new CarModel(car.Id, car.Name, car.Photo.Base64));
            }

            return cars;
        }

        public Task<CarDTO> UpdateCar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
