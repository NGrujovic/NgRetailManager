using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace NRMDataManager.App_Start
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            //Adding parameter to every operation, basicly adding filter to see if user is logged in(if he has a token) and if he is 
            // authorized to use operation
            if(operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name="Authorization",
                @in = "header",
                description = "acess token",
                required = false,
                type = "string"
            });
        }
    }
}