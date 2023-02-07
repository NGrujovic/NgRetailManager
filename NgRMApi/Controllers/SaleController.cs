using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NgRMDataManager.Library.DataAccess;
using NgRMDataManager.Library.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace NgRMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {

            SaleData data = new SaleData();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);

        }

        [HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Admin,Manager")]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData data = new SaleData();

            return data.GetSaleReport();
        }
    }
}
