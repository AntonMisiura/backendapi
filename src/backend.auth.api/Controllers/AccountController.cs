using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using backend.auth.api.Model;
using backend.auth.api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.auth.api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private AccessProvider _accessProvider;

        public AccountController(AccessProvider accessProvider)
        {
            _accessProvider = accessProvider;
        }
        
        [AllowAnonymous, HttpPost("[action]")]
        public async Task<ActionResult> FacebookAuth([FromBody] ExternalLoginModel model)
        {
            try
            {
                await _accessProvider.VerifyAccessToken(model.Email, model.Token);
                
                var result = await _accessProvider.SignInWithFacebook(model.Email);

                return Ok(result);
            }
            catch (ValidationException)
            {
                return BadRequest(new ValidationException("External login model is not valid"));
            }
        }

        [AllowAnonymous, HttpPost("[action]")]
        public async Task<ActionResult> CreateAccountWithFacebook(AccountModel account, string token)
        {
            try
            {
                await _accessProvider.VerifyAccessToken(account.Email, token);

                if (ModelState.IsValid)
                {
                    var result = await _accessProvider.CreateFacebookLogin(account);

                    return Ok(result);
                }

                return BadRequest(ModelState);
            }
            catch (ValidationException)
            {
                return BadRequest(new ValidationException("Model is not valid"));
            }
        }
    }
}
