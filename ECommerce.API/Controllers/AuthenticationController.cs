using ECommerce.API.ApplicationBase;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.LoginUser.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.Logout.Command;
using ECommerce.Application.Features.AuthenticationFeatures.LoginAndTokens.RefreshToken.Model;
using ECommerce.Application.Features.AuthenticationFeatures.Password.RestPassword.Command;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.ConfirmEmail.Command;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterBuyer.Command.Model;
using ECommerce.Application.Features.AuthenticationFeatures.Registeration.RegisterSeller.Command.Model;
using ECommerce.Domain.AuthenticationHepler;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthenticationController : ApplicationControllerBase
    {

        [HttpPost("RegisterBuyer")]
        public async Task<IActionResult> RegisterBuyer([FromForm] AddBuyerUserCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }


        [HttpPost("RegisterSeller")]
        public async Task<IActionResult> RegisterSeller([FromForm] AddSellerUserCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();

        }


        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromForm] UserLogInCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.IsSuccess && result.Value?.RefreshToken != null && result.Value.CookieOptions != null)
            {
                Response.Cookies.Append("RefreshToken", result.Value.RefreshToken, result.Value.CookieOptions);
            }
            return result.ResultStatusCode();
        }

        //[HttpPost("Confirm-Email")]
        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPost("Resend-Confirm-Email")]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] ResendConfirmEmailCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }
        //[HttpGet("Resend-Confirm-Email")]
        //public async Task<IActionResult> ResendConfirmEmail([FromQuery] ConfirmEmailCommand command)
        //{
        //    var result = await Mediator.Send(command);
        //    return result.ResultStatusCode();
        //}
        [HttpPost("Generate-New-token-From-RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await Mediator.Send(new RefreshTokenCommand());

            if (result.IsSuccess && result.Value?.RefreshToken != null && result.Value.CookieOptions != null)
            {
                Response.Cookies.Delete("RefreshToken");
                Response.Cookies.Append("RefreshToken", result.Value.RefreshToken, result.Value.CookieOptions);
            }
            return result.ResultStatusCode();
        }

        [HttpPost("Rest-Password")]
        public async Task<IActionResult> RestPassword([FromForm] ResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword([FromForm] SendTokenToRestPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result.ResultStatusCode();
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await Mediator.Send(new UserLogoutCommand());
            
            return result.ResultStatusCode();
        }


        // To Do
        // 2- Add EndPoint to Get Token To Rest Password

        // 3- Add Endpoint To Change Password From Gmail 

        //using Microsoft.AspNetCore.Identity;
        //[HttpGet("aaa")]
        //    public async Task<IActionResult> C()
        //    {

        //    var hasher = new PasswordHasher<ApplicationUser>();
        //        var user = new ApplicationUser { Id = "admin-user-id", UserName = "admin@ECommerce.com" };
        //        var hashedPassword = hasher.HashPassword(user, "Aa123**");
        //        return Ok(hashedPassword);
        //     }

    }

}
