using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.DataBase;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        

        public async Task<User> RegisterUser(RegisterUserDTO registerUserDTO)
        {

            var emailExists = await _context.Users.AnyAsync(u => u.Email == registerUserDTO.Email);

            if (emailExists) 
            {
                throw new Exception("Email already registered");
            }

            var user = new User
            {
                Email = registerUserDTO.Email,
                FullName = registerUserDTO.FullName,
                UserName = registerUserDTO.UserName,
                Password = string.Empty
            };

            user.Password = new PasswordHasher<User>().HashPassword(null!, registerUserDTO.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<object> LoginUser(string email, string password)
        {
            
            var user = 
                await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null) {
                throw new Exception("User not found");
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid Password");
            }

            var token = GenerateToken(user);

            return new 
            {
                Message = "login success!",
                Token = token 
            };
        }

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWTSetting");

            var securityKey = jwtSettings["securityKey"] 
            ?? throw new ArgumentNullException("securityKey:", "security key is not assigned");

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}