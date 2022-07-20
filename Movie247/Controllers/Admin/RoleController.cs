using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movie247.Areas.Identity.Data;
using Movie247.Helpers;

namespace Movie247.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<Movie247User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<Movie247User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<JsonResult> index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return JsonReturn.Success(roles);
        }
        public async Task<JsonResult> Add(string name)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return JsonReturn.Success("Role added successfully");
            }
            else
            {
                return JsonReturn.Error("Role could not be added because already exists");
            }
        }
        public async Task<JsonResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return JsonReturn.Success("Role deleted successfully");
                }
                else
                {
                    return JsonReturn.Error("Role could not be deleted");
                }
            }
            else
            {
                return JsonReturn.Error("Role could not be found");
            }
        }
        public async Task<JsonResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                role.Name = name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return JsonReturn.Success("Role updated successfully");
                }
                else
                {
                    return JsonReturn.Error("Role could not be updated");
                }
            }
            else
            {
                return JsonReturn.Error("Role could not be found");
            }
        }

    }
}
