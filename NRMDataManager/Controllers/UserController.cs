using Microsoft.AspNet.Identity;
using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
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


        [HttpGet]
        // GET:
        public UserModel GetById()
        {
            //using aspnet.identity getting id of logged in person nad passing it to get data from user
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            return data.GetUserById(userId).First();

            
        }

    }
}
