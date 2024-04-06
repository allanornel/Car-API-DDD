using Domain.Interfaces;
using Domain.Exceptions;
using Application.DTOs;


namespace Application.Services.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<PhotoResult> AddPhoto(PhotoDTO photoDTO)
        {
            if (string.IsNullOrWhiteSpace(photoDTO.Base64))
            {
                throw new ArgumentException("Base64 string cannot be empty or null.");
            }

            int photoId = await _photoRepository.AddAsync(photoDTO.Base64);
            return new PhotoResult(photoId, photoDTO.Base64);
        }

        public Task<PhotoDTO> DeletePhoto(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PhotoResult> GetPhoto(int id)
        {
            var photo = await _photoRepository.GetByIdAsync(id);
            if (photo == null)
            {
                throw new NotFoundException($"Photo with ID {id} not found.");
            }

            return new PhotoResult(photo.Id, photo.Base64);
        }

        public Task<PhotoDTO> UpdatePhoto(int id)
        {
            throw new NotImplementedException();
        }

        Task<PhotoDTO> IPhotoService.GetPhoto(int id)
        {
            throw new NotImplementedException();
        }
    }
}

