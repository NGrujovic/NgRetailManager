using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NgRMApi.Data;
using NgRMApi.Models;
using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        
        private readonly ApplicationDbContext _contexdt;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext contexdt,UserManager<IdentityUser> userManager)
        {
            _contexdt = contexdt;
            _userManager = userManager;
        }
        
        

        [HttpGet]
        // GET:
        public UserModel GetById()
        {
            //using aspnet.identity getting id of logged in person nad passing it to get data from user
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// old way - RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();


        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
                List<ApplicationUserModel> output = new List<ApplicationUserModel>();

                var users = _contexdt.Users.ToList();
            var userRoles = from ur in _contexdt.UserRoles
                            join rol in _contexdt.Roles on ur.RoleId equals rol.Id
                            select new { ur.UserId, ur.RoleId, rol.Name };



                foreach (var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };

                    u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId,val => val.Name);
                    //foreach (var r in user.Roles)
                    //{
                    //    u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    //}
                    output.Add(u);
                }
                return output;
            
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            
                var roles = _contexdt.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
            


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user, pairing.RoleName);
            


        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {

            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);

        }
    }
}
