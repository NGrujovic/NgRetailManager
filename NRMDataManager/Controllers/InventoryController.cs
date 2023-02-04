using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NRMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
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
