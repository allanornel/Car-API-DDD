using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICarRepository
    {
        Task<Car?> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync(string query, int page);
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task<int> DeleteAsync(int id);
    }
}
