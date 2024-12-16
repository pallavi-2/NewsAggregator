using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Data;
using NewsAggregator.Dtos;
using NewsAggregator.Interface;
using NewsAggregator.Models;
using NewsAggregator.Services;

namespace NewsAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(ApplicationDbContext context, ITokenService service)
        {
            _context = context;
            _tokenService = service;
        }

        [HttpPost ("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterDto register )
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x=>x.UserName == register.UserName || x.Email == register.Email);
            if (existingUser != null )
            {
                return Conflict("User with the email already exists");
            }

            if (string.IsNullOrEmpty(register.Password) || string.IsNullOrEmpty(register.UserName) || string.IsNullOrEmpty(register.Email))
            {
                return BadRequest("Please provide all the credentials");
            }

            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreateHash(register.Password, out passwordHash, out passwordSalt);

            User user = new User
            {
                Name = register.UserName,
                Email = register.Email,
                UserName = register.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserResponseDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] LoginDto login)
        {
            if (string.IsNullOrEmpty(login.Password) || string.IsNullOrEmpty(login.Email)) {
                return BadRequest("Please provide all the required credentials");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Email == login.Email);
            if (user == null) {
                return Unauthorized("No such user exists");
            }

            var passwordVerification = PasswordHasher.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt);
            if (passwordVerification)
            {
                return new UserResponseDto
                {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };
            }
            else return BadRequest("Incorrect Credentials");


        }
    }
}
