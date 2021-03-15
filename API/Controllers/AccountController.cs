
using API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if(await UserExist(registerDto.Username)) return BadRequest("UserName is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
        [HttpPost("Login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto){
            var user=await _context.Users.SingleAsync(x=>x.UserName==loginDto.Username);

            if(user==null) return Unauthorized("Invalid User");

            using var hmac=new HMACSHA512(user.PasswordSalt);

            var computeHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i=0;i<computeHash.Length;i++){
                if(computeHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }
        public async Task<bool> UserExist(string username){
             return  await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }

    }
    
}