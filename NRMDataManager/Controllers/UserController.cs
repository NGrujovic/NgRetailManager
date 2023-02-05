using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
using NRMDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace NRMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        List<ApplicationUserModel> output = new List<ApplicationUserModel>();
        [HttpGet]
        // GET:
        public UserModel GetById()
        {
            //using aspnet.identity getting id of logged in person nad passing it to get data from user
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();

            
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var users = userManager.Users.ToList();

                var roles = context.Roles.ToList();

                foreach(var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel
                    {
                        Id = user.Id,
                        Email = user.Email
                    };
                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId, roles.Where(x => x.Id == r.RoleId).First().Name);
                    }
                    output.Add(u);
                }
                return output;
            }
        }

    }
}
