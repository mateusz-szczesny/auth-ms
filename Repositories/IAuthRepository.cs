using System.Threading.Tasks;
using Auth.Models;

namespace Auth.Repositories
{
    public interface IAuthRepository
    {

        Task<Token> Login(string identifier, string password);
        Task<User> Register(string username, string email, string password);
    }
}