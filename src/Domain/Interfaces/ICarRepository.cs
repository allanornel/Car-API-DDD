using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync(string query, int page);
        Task<Car> AddAsync(string name, string base64);
        Task UpdateAsync(Car car);
        Task<int> DeleteAsync(int id);
    }
}
