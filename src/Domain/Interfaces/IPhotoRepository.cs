using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetByIdAsync(int id);
        Task<Photo?> AddAsync(string base64);
    }
}
