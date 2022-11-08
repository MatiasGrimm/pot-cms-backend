using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API
{
    public static class DataExtensions
    {
        /// <summary>
        /// Sets the roles the specified user is a member of to the supplied role names. Any other roles will be removed from the user.
        /// </summary>
        public static async Task SetRolesAsync<T>(this UserManager<T> userManager, T user, IReadOnlyCollection<string> roles)
            where T : class
        {
            IList<string> rolesToRemove;

            if (roles.Count != 0)
            {
                IList<string> existingRoles = await userManager.GetRolesAsync(user);
                await userManager.AddToRolesAsync(user, roles);

                // remove any roles from user that were not specified
                rolesToRemove = existingRoles.Except(roles).ToList();
            }
            else
            {
                // remove all roles from user
                rolesToRemove = await userManager.GetRolesAsync(user);
            }

            if (rolesToRemove.Count > 0)
            {
                await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }
        }
    }
}
