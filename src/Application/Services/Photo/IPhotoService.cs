using Application.DTOs;

namespace Application.Services.Photo
{
    public interface IPhotoService
    {
        Task<PhotoDTO> GetPhoto(int id);
        Task<PhotoResult> AddPhoto(PhotoDTO photoDTO);
        Task<PhotoDTO> UpdatePhoto(int id);
        Task<PhotoDTO> DeletePhoto(int id);
    }
}
