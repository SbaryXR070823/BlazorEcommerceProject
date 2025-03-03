using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PcPartsStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagerDomainController : ControllerBase
    {
        private readonly UserManager<Data.ApplicationUser> _userManager;

        public UserManagerDomainController(UserManager<Data.ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("role")]
        [Authorize]
        public async Task<IActionResult> GetUserRole()
        {
            var user = User;

            if (user == null)
                return NotFound("User not found");

            var userToCheck = await _userManager.GetUserAsync(user);
            if (userToCheck == null)
                return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(userToCheck);
            return Ok(roles);
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName
            });
        }
    }
}