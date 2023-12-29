using Authentication.Services.Abtract;
using Domain.Entity;
using Domain.ViewEntity.Authen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abtract;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IUserServices _userServices;
        ITokenHandle _tokenHandle;
        IUserTokenServices _userTokenServices;
        public AuthenticationController(IUserServices userServices, ITokenHandle tokenHandle, IUserTokenServices userTokenServices)
        {
            _userServices = userServices;
            _tokenHandle = tokenHandle;
            _userTokenServices = userTokenServices;
        }

        [HttpPost("PhoneLogin")]
        public async Task<IActionResult> PhoneLogin([FromBody] PhoneLogin phoneLogin)
        {
            var rs = await _userServices.Login(phoneLogin.NumberPhone, phoneLogin.Password);
            if(!rs.Success)
                return BadRequest(rs.ErrorMessage);

            rs.Data.BaseInfo.UserName = phoneLogin.NumberPhone;

            UserToken userToken = new UserToken();

            (rs.Data.Token, userToken.ExpToken) = await _tokenHandle.CreateToken(rs.Data.BaseInfo);
            (rs.Data.RefreshToken, userToken.ExpRefreshToken, userToken.Name) = await _tokenHandle.CreateRefreshToken(rs.Data.BaseInfo);

            userToken.UserId = await _userServices.GetUserID(rs.Data.BaseInfo.UserName);
            userToken.Value = rs.Data.Token;
            userToken.RefreshToken = rs.Data.RefreshToken;
            userToken.CreateDate = DateTime.Now;
            userToken.LoginProvider = "DEFAULT";
            userToken.IsActive = true;

            await _userTokenServices.CreateToken(userToken);

            return Ok(rs.Data);
        }

        [HttpPost("RegisterByPhone")]
        public async Task<IActionResult> RegisterByPhone([FromBody] UserRegister register)
        {
            register.VUser.UserName = register.VUser.NumberPhone;
            var rs = await _userServices.RegisterUser(register);

            if(rs.Success)
              return Ok(rs.Data);
            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
            if (refreshToken is null) return BadRequest();
            var rs = await _tokenHandle.ValidateRefreshToken(refreshToken);
            if(rs is null) return BadRequest();
            return Ok(rs);
        }
    }
}
