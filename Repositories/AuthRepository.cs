using System;
using System.Threading.Tasks;
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
        private readonly Auth.Models.DbContext _context;
        private readonly IOptions<AuthorizationSettings> _settings;

        public AuthRepository(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            Auth.Models.DbContext context,
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
                var userByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == identifier || x.UserName == identifier);
                if (userByEmail != null && await _userManager.CheckPasswordAsync(userByEmail, password))
                {
                    await _signInManager.SignInAsync(userByEmail, true, null);
                    return new Token(TokenGenerator.GenerateToken(_settings.Value, userByEmail.Id));
                }
                else
                {
                    var userByUserName = await _context.Users.FirstOrDefaultAsync(x => x.UserName == identifier);
                    if (userByUserName != null && await _userManager.CheckPasswordAsync(userByUserName, password))
                    {
                        await _signInManager.SignInAsync(userByEmail, true, null);
                        return new Token(TokenGenerator.GenerateToken(_settings.Value, userByEmail.Id));
                    }
                    else
                    {
                        throw new Exception("Username or password is invalid");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<User> Register(string username, string email, string password)
        {
            try
            {
                var newUser = new Auth.Models.User();
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
                throw new Exception(e.Message);
            }
        }
    }
}