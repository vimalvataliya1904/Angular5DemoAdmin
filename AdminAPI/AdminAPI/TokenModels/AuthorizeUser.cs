using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.TokenModels
{
    public class AuthorizeUser : ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var access_token = context.HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(Convert.ToString(access_token)))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(Convert.ToString(access_token).Split(" ")[1]) as JwtSecurityToken;
                    var roletype = tokenS.Claims.First(claim => claim.Type == "type").Value;
                    if (!Roles.Split('|').Contains(Convert.ToString(roletype[0]).ToUpper()))
                    {
                        var data = new
                        {
                            result = false,
                            statusCode = 403,
                            message = "The account being accessed does not have sufficient permissions to execute this operation."
                        };
                        context.Result = new JsonResult(data);
                    }
                }
                else
                {
                    var data = new
                    {
                        result = false,
                        statusCode = 403,
                        message = "Unauthorize."
                    };
                    context.Result = new JsonResult(data);
                }
            }
            catch (Exception ex)
            {
                var data = new
                {
                    result = false,
                    statusCode = 403,
                    message = "Unauthorize."
                };
                context.Result = new JsonResult(data);
            }
        }
    }
}
