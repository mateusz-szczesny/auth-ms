using System.Threading.Tasks;
using Auth.Models;

namespace Auth.Services
{
    public interface IAuthService
    {
        Task<Token> Login(string identifier, string password);
        Task<User> Register(string username, string email, string password);
    }
}