using backend.auth.api.Entities;
using Microsoft.AspNetCore.Mvc;
using backend.auth.api.Contracts;
using backend.auth.api.Service;


namespace backend.auth.api.Controllers
{
    [Route("api/[controller]")]
    public class SignUpController : Controller
    {
        //private readonly ISignUpService _signUpService = new SignUpService(null);

        //:TODO:dependency injection doesn't works fine
        private ISignUpService _signUpService;

        public SignUpController(ISignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        // POST api/signup
        [HttpPost]
        public void CreateUser([FromBody]User user)
        {
            _signUpService.CreateUser(user);
        }
    }
}
