using api.DataBase;
using api.Dtos;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class AccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
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

        public async Task<string> LoginUser(string email, string password)
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

            return "Login";
        }
    }
}