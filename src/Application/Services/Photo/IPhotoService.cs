using Application.DTOs;

namespace Application.Services.Photo
{
    public interface IPhotoService
    {
        Task<Domain.Entities.Photo> GetPhoto(int id);
        Task<Domain.Entities.Photo?> AddPhoto(PhotoDTO photoDTO);
    }
}
