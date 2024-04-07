using Application.DTOs;
using Application.Models;

namespace Application.Services.Car
{
    public interface ICarService
    {
        Task<PaginationModel<CarModel>> GetCars(string query, int page);
        Task<CarModel> GetCar(int id);
        Task<Domain.Entities.Car> AddCar(CarDTO carDTO);
        Task UpdateCar(int id, CarDTO carDTO);
        Task DeleteCar(int id);
    }
}
