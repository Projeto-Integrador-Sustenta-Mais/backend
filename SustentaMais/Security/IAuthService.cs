using SustentaMais.Model;

namespace SustentaMais.Security
{
    public interface IAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}