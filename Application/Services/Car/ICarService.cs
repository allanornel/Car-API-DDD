using Application.DTOs;
using Application.Models;

namespace Application.Services.Car
{
    public interface ICarService
    {
        Task<List<CarModel>> GetCars(string query, int page);
        Task<CarModel> GetCar(int id);
        Task<CarResult> AddCar(CarDTO carDTO);
        Task<CarDTO> UpdateCar(int id);
        Task DeleteCar(int id);
    }
}
