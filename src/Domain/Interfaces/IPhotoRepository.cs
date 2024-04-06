using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetByIdAsync(int id);
        Task<int> AddAsync(string base64);
    }
}
