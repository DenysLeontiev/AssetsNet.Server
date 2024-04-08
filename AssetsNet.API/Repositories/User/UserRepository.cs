using AssetsNet.API.Data;
using AssetsNet.API.DTOs.Photo;
using AssetsNet.API.Entities;
using AssetsNet.API.Helpers;
using AssetsNet.API.Interfaces.Photo;
using AssetsNet.API.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AssetsNet.API.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly AssetsDbContext _context;
    private readonly IPhotoService _photoService;

    public UserRepository(AssetsDbContext context,
        IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    public async Task UpdateUserRequestsLimitAsync(TariffPlansEnum tariff, int paymentState, string userId)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId))
            ?? throw new Exception("User is not found");

        if (paymentState is not TariffPlanConsts.SuccessPaymentState)
        {
            throw new Exception("Payment state is not success");
        }

        if (userToUpdate.GptRequestsLeft > 0)
        {
            throw new Exception("User has enought Gpt requests");
        }
        else
        {
            userToUpdate.GptRequestsLeft = tariff switch
            {
                TariffPlansEnum.Basic => TariffPlanConsts.BasicGptRequests,
                TariffPlansEnum.Premium => TariffPlanConsts.PremiumGptRequests,
                _ => throw new Exception("Tariff plan is not found")
            };

            await _context.SaveChangesAsync();
        }
    }

    public async Task<Photo> UploadProfilePhotoAsync(IFormFile file, string userId)
    {
        var userToUpdateProfilePhoto = await _context.Users.Include(x => x.ProfilePhoto)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId)) ?? throw new Exception("User with is not found");


        if (userToUpdateProfilePhoto!.ProfilePhoto is not null)
        {
            var result = await _photoService.DeleteAsync(userToUpdateProfilePhoto.ProfilePhoto.PublicId);

            if (result.Error is not null)
            {
                throw new Exception(result.Error.Message);
            }
        }

        var photo = await _photoService.UploadAsync(file);

        userToUpdateProfilePhoto!.ProfilePhoto = photo;

        await _context.SaveChangesAsync();

        return photo;
    }
}