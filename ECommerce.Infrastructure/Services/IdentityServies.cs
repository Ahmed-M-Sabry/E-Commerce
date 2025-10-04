using ECommerce.Application.Comman;
using ECommerce.Application.IServices;
using ECommerce.Domain.AuthenticationHepler;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
public class IdentityServies : IIdentityServies
{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSittings _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityServies(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            JwtSittings jwt, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateJwtToken(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResponseAuthModel> RefreshTokenAsunc(string token)
        {
            var responseAuthModel = new ResponseAuthModel();
            var user = await _userManager.Users
                .Include(u => u.refreshTokens)
                .SingleOrDefaultAsync(u => u.refreshTokens.Any(t => t.token == token));

            if (user == null)
            {
                responseAuthModel.Message = "User Not Found";
                return responseAuthModel;
            }

            var refreshToken = user.refreshTokens.Single(t => t.token == token);

            if (!refreshToken.IsActive)
            {
                responseAuthModel.Message = "Inactive Token";
                return responseAuthModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.refreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var jwtToken = await CreateJwtToken(user);

            responseAuthModel.Token = jwtToken;
            var roles = await _userManager.GetRolesAsync(user);
            responseAuthModel.Roles = roles.ToList();
            responseAuthModel.RefreshToken = newRefreshToken.token;
            responseAuthModel.RefreshTokenExpiration = newRefreshToken.ExpireOn;
            responseAuthModel.CookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.ExpireOn,
                Path = "/"
            };

            return responseAuthModel;
        }

        public async Task<bool> RevokeRefreshTokenFromCookiesAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var user = await _userManager.Users
                .Include(u => u.refreshTokens)
                .FirstOrDefaultAsync(u => u.refreshTokens.Any(t => t.token == refreshToken));

            if (user == null)
                return false;

            var token = user.refreshTokens.SingleOrDefault(t => t.token == refreshToken);

            if (token == null || !token.IsActive)
                return false;

            token.RevokedOn = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken", new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return true;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<ResponseAuthModel> GenerateRefreshTokenAsync(ApplicationUser user, bool rememberMe, CancellationToken cancellationToken = default)
        {
            var jwtToken = await CreateJwtToken(user);

            RefreshToken refreshToken;

            var existingActiveToken = user.refreshTokens.FirstOrDefault(r => r.IsActive);

            if (existingActiveToken != null)
            {
                refreshToken = existingActiveToken;
            }
            else
            {
                refreshToken = GenerateRefreshToken();

                if (rememberMe)
                    refreshToken.ExpireOn = DateTime.UtcNow.AddDays(30);

                user.refreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new ResponseAuthModel
            {
                Message = "Login successful.",
                Token = jwtToken,
                Roles = roles.ToList(),
                RefreshToken = refreshToken.token,
                RefreshTokenExpiration = refreshToken.ExpireOn,
                CookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpireOn,
                    Path = "/"
                }
            };
        }

        public async Task<bool> IsEmailExist(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> IsInRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<ApplicationUser> IsUserExist(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;
            return user;
        }
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;
            return user;
        }


        public async Task<IdentityResult> CreateSellerUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
        {
            await _userManager.CreateAsync(user, password);
            return await _userManager.AddToRoleAsync(user, ApplicationRoles.Seller);
        }
        public async Task<IdentityResult> CreateBuyerUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
        {
            await _userManager.CreateAsync(user, password);
            return await _userManager.AddToRoleAsync(user, ApplicationRoles.Buyer);
        }

        public async Task<bool> IsPasswordExist(ApplicationUser user, string Password, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.CheckPasswordAsync(user, Password);
            return result;
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var result =  await _userManager.FindByEmailAsync(email);

            return result;
        }

        public async Task<string> GetEmailConfirmationTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            //var token =  await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //return  WebUtility.UrlEncode(token);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenBytes);
        }
        public async Task<IdentityResult> ConfirmEmailByTokenAsync(ApplicationUser user, string decodedToken, CancellationToken cancellationToken = default)
        {
            return await _userManager.ConfirmEmailAsync(user, decodedToken);
        }

        public async Task<string> GetRestPasswordTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            return WebEncoders.Base64UrlEncode(tokenBytes);
        }
        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        
    }
}
