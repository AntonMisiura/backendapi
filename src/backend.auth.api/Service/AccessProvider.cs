using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using backend.auth.api.Entities;
using backend.auth.api.Model;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using backend.auth.api.Repo;

namespace backend.auth.api.Service
{
    public class AccessProvider
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private TokenRepository _tokenRepository;
        private SignInManager<User> _signInManager;
        

        public AccessProvider(UserManager<User> userManager, SignInManager<User> signInManager, TokenRepository tokenRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenRepository = tokenRepository;
        }


        //TODO: Make LoginHandler for facebook google and own, check data for valid if exist create jwt, create user in middlewareToken
        //Це буде інтерфейс ILoginHandler, і від нього будуть наслєдуватись FBLoginHandler, GoogleLoginHandler, OwnLoginHandler
        //в ньому буде перевірятись юзер на валідацію, в случаї гугла і фейсбука будуть перевірятись аксес токєни
        //а в случаї звичайної реєстрації будуть просто валідіруватись дані пользоватєля, потім в мідлвере я буду создавати юзера, і писати в базу за допомогою репозіторія
        //і в мідлвере дальше після провєрки буду генерити токєн і передавати його маші
        public async Task<FacebookMeResponse> VerifyAccessToken(string email, string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ValidationException("Invalid Facebook token");
            }

            var facebookGraphUrl = "https://graph.facebook.com/me?fields=cover,age_range,first_name,location,last_name,hometown,gender,birthday,email&access_token=" + accessToken;
            var request = WebRequest.Create(facebookGraphUrl);
            request.Credentials = CredentialCache.DefaultCredentials;

            using (var response = await request.GetResponseAsync())
            {
                var status = ((HttpWebResponse)response).StatusCode;

                var dataStream = response.GetResponseStream();

                var reader = new StreamReader(dataStream);
                var responseFromServer = reader.ReadToEnd();
                var facebookUser = JsonConvert.DeserializeObject<FacebookMeResponse>(responseFromServer);

                var valid = facebookUser != null && !string.IsNullOrWhiteSpace(facebookUser.Email) && facebookUser.Email.ToLower() == email.ToLower();
                facebookUser.PublicProfilePhotoUrl = "http://graph.facebook.com/" + facebookUser.Id + "/picture";

                if (!valid)
                {
                    throw new ValidationException("Invalid Facebook token");
                }

                return facebookUser;
            }
        }

        public async Task<FacebookResponse> SignInWithFacebook(string email)
        {
            var claims = new List<Claim>();

            var user = await _userManager.FindByEmailAsync(email);

            var identity = new ClaimsIdentity(claims, "oidc");

            var jwtBearerToken = Guid.NewGuid().ToString();
            var properties = new AuthenticationProperties();
            properties.Items.Add(".Token.access_token", jwtBearerToken);

            await _signInManager.SignInAsync(user, properties, "oidc");

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var token = new Token
            {
                Key = jwtBearerToken,
                Expiry = DateTime.UtcNow.AddMinutes(30),
                UserId = user.Id,
                TokenType = "FacebookLogin"
            };

            await _tokenRepository.Save(token);
            var result = _signInManager.IsSignedIn(principal);

            return new FacebookResponse("success", result, jwtBearerToken);
        }

        public async Task<FacebookResponse> CreateFacebookLogin(AccountModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                var createResult = await _userManager.CreateAsync(_mapper.Map<AccountModel, User>(model));
                if (!createResult.Succeeded)
                {
                    // handle failure..
                }
            }

            return await SignInWithFacebook(model.Email);
        }
    }
}
