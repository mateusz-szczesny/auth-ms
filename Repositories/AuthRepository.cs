using System;
using System.Threading.Tasks;
using Auth.Crypto;
using Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Auth.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DatabaseContext _context;
        private readonly IOptions<AuthorizationSettings> _settings;

        public AuthRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            DatabaseContext context,
            IOptions<AuthorizationSettings> settings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _settings = settings;
        }

        public async Task<Token> Login(string identifier, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == identifier);
                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    await _signInManager.SignInAsync(user, true);
                    return new Token(TokenEncryptor.GenerateToken(_settings.Value, user.UserName));
                }
                else
                {
                    throw new Exception("Username or password is invalid");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<User> Register(string username, string email, string password)
        {
            try
            {
                var newUser = new User();
                newUser.UserName = username;
                newUser.Email = email;
                var result = await _userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
                    return user;
                }
                else
                {
                    throw new Exception("Username or password is invalid");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}