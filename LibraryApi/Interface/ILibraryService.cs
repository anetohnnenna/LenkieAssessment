using LibraryApi.Entities;
using LibraryApi.Models;

namespace LibraryApi.Interface
{
    public interface ILibraryService 
    {
        Task<GenericApiResponse<RegistrationModel>> UserReg(RegistrationModel reg);
        Task<GenericApiResponse<LoginResponse>> UserLogin(LoginRequest userReq);
        Task<GenericApiResponse<IEnumerable<LibraryBook>>> GetAvaliableBooks();
        Task<GenericApiResponse<BookOrder>> ReserveBook(ReservedBookRequest userReq);
        Task<GenericApiResponse<BookOrder>> BorrowBook(BorrowBookRequest userReq);
        Task<GenericApiResponse<BookOrder>> ReturnBook(BorrowBookRequest userReq);
        Task<GenericApiResponse<BookOrder>> ResetLibraryBookStatus();
        string Encryptdata(string password);
        string Decryptdata(string encryptpwd);
    }
}
