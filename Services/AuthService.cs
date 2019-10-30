using System.Threading.Tasks;
using Auth.Models;
using Auth.Repositories;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Token> Login(string identifier, string password)
        {
            return await _authRepository.Login(identifier, password);
        }

        public async Task<User> Register(string username, string email, string password)
        {
            return await _authRepository.Register(username, email, password);
        }
    }
}