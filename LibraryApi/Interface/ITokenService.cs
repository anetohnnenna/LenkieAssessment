using IdentityModel.Client;

namespace LibraryApi.Interface
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}
