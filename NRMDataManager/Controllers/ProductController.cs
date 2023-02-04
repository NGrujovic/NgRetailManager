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
    [Authorize(Roles = "Cashier")]
    public class ProductController : ApiController
    {
        // GET api/product
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData();

            return data.GetAllProducts();

        }
    }
}
