using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Exceptions;

namespace ZonkGame.DB.Utils
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser> GetUserById(
            this Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> manager,
            Guid UserId)
        {
            return await manager.Users
                .Where(x => x.Id == UserId)
                .FirstOrDefaultAsync() ??
                    throw new EntityNotFoundException("User",
                    new()
                    {
                        ["Id"] = UserId.ToString(),
                    });
        }
    }
}
