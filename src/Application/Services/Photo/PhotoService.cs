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

        public async Task<Domain.Entities.Photo?> AddPhoto(PhotoDTO photoDTO)
        {
            if (string.IsNullOrWhiteSpace(photoDTO.Base64))
            {
                throw new ArgumentException("Base64 string cannot be empty or null.");
            }
            return await _photoRepository.AddAsync(photoDTO.Base64); ;
        }

        public async Task<Domain.Entities.Photo> GetPhoto(int id)
        {
            var photo = await _photoRepository.GetByIdAsync(id);
            if (photo == null)
            {
                throw new NotFoundException($"Photo with ID {id} not found.");
            }

            return photo;
        }
    }
}

