using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace NRMDataManager.App_Start
{
    public class AuthTokenOperation : IDocumentFilter
    {
        //In this class i configure swagger to display and accept one more path, which is /token. This path will be our login
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            //adding route
            swaggerDoc.paths.Add("/token", new PathItem
            {
                //Command will be POST
                post = new Operation
                {
                    //Put it in auth cattegory
                    tags = new List<string> { "Auth" },
                    //params that user is sending will come through application/x-www-form-urlencoded
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    //adding 3 parameters which user needs to send (Definition)
                    parameters = new List<Parameter> {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",
                            @default = "password"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = false,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "password",
                            required = false,
                            @in = "formData"
                        }
                    }

                }
            });
        }
    }
}