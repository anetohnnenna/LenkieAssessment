using IdentityModel.Client;
using LibraryApi.Entities;
using LibraryApi.Interface;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public readonly ILibraryService _libraryService;

        public AuthenticateController(IConfiguration configuration, ILibraryService libraryService)
        {
            _configuration = configuration;
            _libraryService = libraryService;
        }


        [Route("GetToken")]
        [HttpGet]
        public async Task<TokenResponse> GetToken(string scope)
        {
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

    
        [Route("Registration")]
        [HttpPost]
        public async Task<IActionResult> Registration(RegRequestModel reg)
        {
            if (ModelState.IsValid)
            {
                RegistrationModel request = new RegistrationModel();
                request.FirstName = reg.FirstName;
                request.LastName = reg.LastName;
                request.Email = reg.Email;
                request.Password = reg.Password;
                request.Address = reg.Address;
                request.PhoneNumber = reg.PhoneNumber;

                GenericApiResponse<RegistrationModel> result = await _libraryService.UserReg(request);
                return Ok(result);
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest userReq)
        {
            try
            {
                GenericApiResponse<LoginResponse> result = new GenericApiResponse<LoginResponse>();
                if (ModelState.IsValid)
                {
                    TokenResponse token = await GetToken(userReq.Scope);
                    if (token.AccessToken != null)
                    {
                        result = await _libraryService.UserLogin(userReq);
                        result.Data.Access_Token = token.AccessToken;
                        result.Data.Expires_in = token.ExpiresIn;
                        result.Data.Token_type = token.TokenType;
                    }
                    else
                    {
                        result.ResponseDescription = "Unable to login successfully, You do not have access to this application, Please contact the admin. Thank you.";
                        result.ResponseCode = ResponseCodes.Failure;
                    }
                  
                }
                else
                {
                    return BadRequest("Kindly input your details");

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
