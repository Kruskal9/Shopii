using Domain.BaseEntity;
using Domain.Entity;
using Domain.ViewEntity.Authen;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using Service.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserServices:IUserServices
    {
        UserManager<User> _userManager;

        public UserServices(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResultData<Profile>> Login(string userName, string password)
        {
            var identityUser = await _userManager.FindByNameAsync(userName);

            if (identityUser is null)
                return new ResultData<Profile>().FalseResult("Your account was not found");
            
            if(!await _userManager.CheckPasswordAsync(identityUser, password))
                return new ResultData<Profile>().FalseResult("Wrong password");

            VUser vUser = new VUser();

            return new ResultData<Profile>().SuccessResult(
                new Profile { BaseInfo = vUser.MapToThis(identityUser) });
        }

        public async Task<ResultData<VUser>> RegisterUser(UserRegister register)
        {
            if(!register.Password.Equals(register.RePassword))
                return new ResultData<VUser>().FalseResult("Password and RePassword are not the same");

            var result = await _userManager.CreateAsync(register.VUser.MapToUser(), register.Password);

            if(!result.Succeeded)
                return new ResultData<VUser>().FalseResult("Can not create account");

            return new ResultData<VUser>().SuccessResult(register.VUser);
        }

        public async Task<bool> CheckExist(string username)
        {
            var identityUser = await _userManager.FindByNameAsync(username);
            if (identityUser is null) return false;
            return true;
        }

        public async Task<string> GetUserID(string username)
        {
            var identityUser = await _userManager.FindByNameAsync(username);
            if (identityUser is null) return null;
            return identityUser.Id;
        }
        public async Task<VUser> GetUser(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser is null) return null;
            return new VUser().MapToThis(identityUser);
        }
    }
}
