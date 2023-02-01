using NgRMDataManager.Library.Internal.DataAccess;
using NgRMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgRMDataManager.Library.DataAccess
{
    public class ProductData
    {
        

        public List<ProductModel> GetAllProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();
            
            
            var output = sql.LoadData<ProductModel>("dbo.spProduct_GetAll", "NgRmDataConnection");
            
            return output;
        }
        public ProductModel GetAllProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();


            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById",new { Id = productId }, "NgRmDataConnection").FirstOrDefault();

            return output;
        }
    }
}
