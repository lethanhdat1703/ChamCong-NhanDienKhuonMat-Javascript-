using ChamCong_BackEnd.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace ChamCong_BackEnd.Server.Repository
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
    }
}
