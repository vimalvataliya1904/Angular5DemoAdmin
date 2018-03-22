using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.Utility;

namespace AdminAPI.TokenModels
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options)
        {
            _next = next;

            _options = options.Value;
            ThrowIfInvalidOptions(_options);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {

            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }


            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }


            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            try
            {
                string username = Convert.ToString(context.Request.Form["username"]);
                string password = Convert.ToString(context.Request.Form["password"]);
                string type = Convert.ToString(context.Request.Form["type"]);
                // var identity = await _options.IdentityResolver(username, password);
                using (var _context = new AdminDemoContext())
                {
                    try
                    {
                        bool data = false;
                        int id = 0;
                        AdminLogins admindata = new AdminLogins();
                        if (type == "admin")
                        {
                            admindata = _context.AdminLogins.Where(x => x.Username == username && x.Password == password && x.IsActive == true).FirstOrDefault();
                            if (admindata != null)
                            {
                                data = true;
                                id = admindata.AdminId;
                            }
                        }
                        if (data != false)
                        {
                            var identity = Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));


                            var now = DateTime.UtcNow;
                            var claims = new Claim[]
                            {
                            new Claim("type",type),
                            new Claim("id",id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, username),
                            new Claim(JwtRegisteredClaimNames.Jti, await _options.NonceGenerator()),
                            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                            };

                            // Create the JWT and write it to a string
                            var jwt = new JwtSecurityToken(
                                issuer: _options.Issuer,
                                audience: _options.Audience,
                                claims: claims,
                                notBefore: now,
                                expires: now.Add(_options.Expiration),
                                signingCredentials: _options.SigningCredentials);
                            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                            if (type == "admin")
                            {
                                var response = new
                                {
                                    status = "success",
                                    access_token = encodedJwt,
                                    expires_in = (int)_options.Expiration.TotalSeconds,
                                    firstname = admindata.FirstName,
                                    lastname = admindata.LastName,
                                    username = admindata.Username,
                                    password = admindata.Password,
                                    usertype = type
                                };
                                // Serialize and return the response
                                context.Response.ContentType = "application/json";
                                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 200;
                            var response = new
                            {
                                status = "error",
                                msg = "Invalid username or password."
                            };
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }
    }
}
