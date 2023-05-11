using LibraryApi.Entities;
using LibraryApi.Interface;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibraryAttendanceController : ControllerBase
    {
        public readonly ILibraryService _libraryService;

        public LibraryAttendanceController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [Route("BorrowBook")]
        [HttpPost]
        public async Task<IActionResult> BorrowBook(BorrowBookRequest userReq)
        {
            if (ModelState.IsValid)
            {
                GenericApiResponse<BookOrder> result = await _libraryService.BorrowBook(userReq);
                return Ok(result);
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }

        [Route("ReturnBook")]
        [HttpPost]
        public async Task<IActionResult> ReturnBook(BorrowBookRequest userReq)
        {
            if (ModelState.IsValid)
            {
                GenericApiResponse<BookOrder> result = await _libraryService.ReturnBook(userReq);
                return Ok(result);
            }
            else
            {
                return BadRequest("Kindly input your details");

            }

        }


        [Route("ResetLibraryBookStatus")]
        [HttpPost]
        public async Task<IActionResult> ResetLibraryBookStatus()
        {
          
                GenericApiResponse<BookOrder> result = await _libraryService.ResetLibraryBookStatus();
                return Ok(result);
    
        }
    }
}
