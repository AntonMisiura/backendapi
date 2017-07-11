using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.auth.api.Contracts;
using backend.auth.api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace backend.auth.api.Security.JWT
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoginHandler[] _handlers;
        private readonly ISignUpService _loginService;
        private readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options,
            ISignUpRepository repository)
        {
            _next = next;
            _options = options.Value;
            _loginService = new SignUpService(repository);
            _handlers = new ILoginHandler[]
            {
                new AllegroLoginHandler(),
                new FacebookLoginHandler(),
                new GoogleLoginHandler()   
            };

        }

        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
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
            var email = context.Request.Form["email"];
            var password = context.Request.Form["password"];
            var loginType = context.Request.Form["logintype"];
            var loginData = context.Request.Form["logindata"];

            var loginHandler = _handlers.FirstOrDefault(h => h.LoginType == loginType);
            // bad request if handler null
            
            var user = loginHandler.GetUser(loginData);

            //TODO:verify token, email or user data
            var dbUser = _loginService.GetOrCreate(user);

            if (dbUser == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                // Mandatory
                new Claim(JwtRegisteredClaimNames.Sub, dbUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),

                // My claims
                new Claim("AlegroUserName", dbUser.UserName)
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

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        //private Task<ClaimsIdentity> GetIdentity(string email, string password)
        //{
        //    // DON'T do this in production, obviously!
        //    if (email == "TestUsername" && password == "TESTPassword123")
        //    {
        //        return Task.FromResult(new ClaimsIdentity(new System.Security.Principal.GenericIdentity(email, "Token"), new Claim[] { }));
        //    }

        //    // Credentials are invalid, or account doesn't exist
        //    return Task.FromResult<ClaimsIdentity>(null);
        //}
    }
}
