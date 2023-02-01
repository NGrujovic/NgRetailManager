using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgRMDataManager.Library
{
    public class ConfigHelper 
    {
        //TODO Move this from config to API
        public static decimal GetTaxRate()
        {


            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = Decimal.TryParse(rateText, out decimal taxRate);
            if (isValidTaxRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return taxRate;
        }
    }
}
