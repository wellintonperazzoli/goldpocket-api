using GoldPocket.Models;
using GoldPocket.Models.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoldPocket.Services
{
    public class AuthService
    {
        UserManager<appUser> _userManager;
        SignInManager<appUser> _signInManager;
        IConfiguration _config;

        public AuthService(UserManager<appUser> userManager, SignInManager<appUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<appUser> GetUser(appUser appUser)
        {
            var user = await _userManager.FindByEmailAsync(appUser.Email);
            if (user == null) throw new ApplicationException("Invalid user");
            var valid = await _signInManager.CheckPasswordSignInAsync(user, appUser.PasswordHash, false);
            return valid.Succeeded ? user : null;
        }

        public async Task<SignInResult> ValidateUser(appUser appUser)
        {
            var user = await _userManager.FindByEmailAsync(appUser.Email);
            var valid = await _signInManager.CheckPasswordSignInAsync(user, appUser.PasswordHash, false);
            return valid;
        }

        public async Task<IdentityResult> Create(appUser appUser)
        {
            var result = await _userManager.CreateAsync(appUser, appUser.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.FindByEmailAsync(appUser.Email);
            }
            return result;
        }

        public async Task<string> GenerateToken(appUser identityUser)
        {
            var user = await GetUser(identityUser);
            if (user == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddYears(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                return null;
            }
        }

    }
}
