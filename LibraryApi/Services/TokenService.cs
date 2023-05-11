using IdentityModel.Client;
using LibraryApi.Interface;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TokenResponse> GetToken(string scope)
        {
            string bb = _configuration.GetValue<string>("tokenUrl");

            using (var client = new HttpClient())
            {
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _configuration.GetValue<string>("tokenUrl"),
                    ClientId = _configuration.GetValue<string>("ClientId"),
                    Scope = scope,
                    ClientSecret = _configuration.GetValue<string>("ClientSecret")
                });

                if (tokenResponse.IsError)
                {
                    throw new Exception("Token Error");
                }
                return tokenResponse;
            }
        }


    }
}
