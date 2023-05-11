using LibraryApi.Entities;
using LibraryApi.Interface;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LibraryUserController : ControllerBase
    {
        public readonly ILibraryService _libraryService;

        public LibraryUserController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
              


        [Route("GetAvaliableBook")]
        [HttpGet]
        public async Task<IActionResult> GetAvaliableBook()
        {
           //fetch all avaliable books with status 1
            GenericApiResponse<IEnumerable<LibraryBook>> result = await _libraryService.GetAvaliableBooks();
                return Ok(result);

        }

        [Route("ReserveBook")]
        [HttpPost]
        public async Task<IActionResult> ReserveBook(ReservedBookRequest userReq)
        {
            if (ModelState.IsValid)
            {
                GenericApiResponse<BookOrder> result = await _libraryService.ReserveBook(userReq);
                return Ok(result);
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }
    }
}
