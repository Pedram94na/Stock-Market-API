using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly SignInManager<AppUser> signinManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signinManager)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signinManager = signinManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser {
                    UserName = register.UserName,
                    Email = register.Email
                };

                var createdUser = await userManager.CreateAsync(appUser, register.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(appUser, "User");

                    if (!roleResult.Succeeded)
                        return StatusCode(500, roleResult.Errors);
                    
                    return Ok(
                        new NewUserDto {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = tokenService.CreateToken(appUser)
                        }
                    );
                }
                
                return StatusCode(500, createdUser.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());

            if (user is null)
                return Unauthorized("Invalid username!");

            var result = await signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Username not found and/or password incorrect!");

            return Ok(
                new NewUserDto {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = tokenService.CreateToken(user)
                }
            );
        }
    }
}