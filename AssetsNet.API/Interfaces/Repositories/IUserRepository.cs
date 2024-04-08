using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Helpers;

namespace AssetsNet.API.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Entities.Photo> UploadProfilePhotoAsync(IFormFile file, string userId);

    Task UpdateUserRequestsLimitAsync(TariffPlansEnum tariff, int paymentState, string userId);
}