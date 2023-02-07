using NgRMDataManager.Library.Internal.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NgRMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;

namespace NgRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        private readonly IConfiguration _config;

        public SaleData(IConfiguration config)
        {
            _config = config;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            
            ProductData prodData = new ProductData(_config);
            var taxRate = ConfigHelper.GetTaxRate();
            //Start filling in the sale detail models we will save to DB
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            foreach(var item in saleInfo.SaleDetails)
            {
                
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    
                };
                //Get the info about  this product
                var productInfo = prodData.GetAllProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product id of {detail.ProductId} could not be found in the DB");
                }
                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * (taxRate / 100));
                }

                details.Add(detail);
            }


            //Create the Sale Model
            SaleDBModel sale = new SaleDBModel
            {
                CashierId = cashierId,
                SubTotal = details.Sum(x=>x.PurchasePrice),
                Tax = details.Sum(x => x.Tax)
            };
            sale.Total = sale.SubTotal + sale.Tax;
            // Save sale model
            
            //Transaction method is method that should not be used every time, since its dangerous if we 
            // leave open transaction
            using(SqlDataAccess sql = new SqlDataAccess(_config))
            {
                try
                {
                    sql.StartTransaction("NgRmDataConnection");

                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    //Get the ID from the sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSale_LastIdLookup", new { }).FirstOrDefault();
                    // Finish filling in the sale detail model
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        //Save sale detail models
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }
                    sql.CommitTransaction();
                    
                }
                catch
                {

                    sql.RollbackTransaction();
                    //Tell the user transaction didnt work
                    throw;
                }
                
            }

            //TODO lover quantity of purchused products

        }

        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<SaleReportModel>("dbo.spSale_SaleReport", "NgRmDataConnection");

            return output;
        }
        
    }
}
