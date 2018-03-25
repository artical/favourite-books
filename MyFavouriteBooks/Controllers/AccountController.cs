using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFavouriteBooks.Models;
using MyFavouriteBooks.Models.Account;
using Newtonsoft.Json;

namespace MyFavouriteBooks.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
          UserManager<User> userManager,
          SignInManager<User> signInManager)
          //IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_options = optionsAccessor.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    User appUser = new User
                    {
                        UserName = model.Username
                    };
                    IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                    if (result.Succeeded)
                    {
                        return Ok("success");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("error", error.Description);
                        }
                    }
                }
                else
                    ModelState.AddModelError("error", "Invalid username or password");
            }
            return BadRequest(ModelState);
        }


        [AllowAnonymous]
        [HttpPost("token")]
        public async Task Token([FromBody] Credentials credentials)
        {
            var username = credentials.Username;
            var password = credentials.Password;

            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid login or password.");
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    //audience: AuthOptions.AUDIENCE,
                    //notBefore: now,
                    claims: (await GetClaims(username))?.Claims ?? null,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                //encodedJwt
                token = "Bearer " + encodedJwt,
                //username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            Response.Headers.Add("Authorization","Bearer " + encodedJwt);
            await Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private async Task<ClaimsIdentity> GetClaims(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token");
                //new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                //    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}