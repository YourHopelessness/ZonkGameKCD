using Microsoft.EntityFrameworkCore;
using ZonkGame.DB.Context;
using ZonkGame.DB.Entites.Auth;
using ZonkGame.DB.Exceptions;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGame.DB.Repositories.Services
{
    public class PermissionRepository(IDbContextFactory<AuthContext> dbFactory) : IPermissionRepository
    {
        private readonly AuthContext authDb = dbFactory.CreateDbContext();
        /// <summary>
        /// Retrieves a permission entity by its name.
        /// </summary>
        /// <param name="name">Permission name</param>
        /// <returns>Permission entity</returns>
        public async Task<Permission> GetPermissionByName(string name)
        {
            return await authDb.Permissions
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync() 
                    ?? throw new EntityNotFoundException("Permission",
                    new()
                    {
                        ["Name"] = name
                    });
        }
    }
}
