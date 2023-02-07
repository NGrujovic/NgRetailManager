using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
using System.Collections.Generic;

namespace NgRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {


        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public List<InventoryModel> Get()
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    do Admin stuff
            //}else if(RequestContext.Principal.IsInRole("Manager")){
            //    do manager stuff
            //}

            InventoryData data = new InventoryData();

            return data.GetInventory();
        }

        [HttpPost]

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel item)
        {

            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(item);
        }
    }

}
