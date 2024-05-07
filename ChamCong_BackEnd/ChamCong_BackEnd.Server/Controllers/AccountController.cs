using ChamCong_BackEnd.Server.Models;
using ChamCong_BackEnd.Server.Repository;
using ChamCong_BackEnd.Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChamCong_BackEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;
        public AccountController(IAccountRepository repo) 
        {
            accountRepo=repo;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result=await accountRepo.SignUpAsync(model);
            if (result.Succeeded) {
                return Ok(result.Succeeded);
            }
            return StatusCode(500);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result =await accountRepo.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
