using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI
{
    public class TokenController: Controller
    {
        public int UserId { get { return GetUserId(); } }
        private int GetUserId()
        {
            try
            {
                var access_token = HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(Convert.ToString(access_token)))
                {                    
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(Convert.ToString(access_token).Split(" ")[1]) as JwtSecurityToken;
                    int id = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "id").Value);
                    return id;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
