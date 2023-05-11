using LibraryApi.Interface;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Security;
using LibraryApi.Entities;
using LibraryApi.Controllers;

namespace LibraryApi.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IRepository<RegistrationModel> _registration;
        private readonly IRepository<LibraryBook> _librarybooks;
        private readonly IRepository<BookOrder> _bookOrder;


        public LibraryService(IRepository<RegistrationModel> registration, IRepository<LibraryBook> librarybooks, IRepository<BookOrder> bookOrder)
        {
            _registration = registration;
            _librarybooks = librarybooks;
            _bookOrder = bookOrder;
        }

        public async Task<GenericApiResponse<RegistrationModel>> UserReg(RegistrationModel reg)
        {
            GenericApiResponse<RegistrationModel> result = new GenericApiResponse<RegistrationModel>();
            try
            {               
                //check if the user is existing
                RegistrationModel details = await _registration.FetchRecord(m => m.Email == reg.Email);

                //if it returns no record, then it means the user is a not existing
                if (details == null)
                {
                    string password = Encryptdata(reg.Password);
                    reg.Password = password;
                    await _registration.Add(reg);
                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = "You have registered suceessfully, kindly login to proceed. Thank you.";

                }
                else
                {
                    result.ResponseDescription = "You have registered before, kindly login to proceed. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<LoginResponse>> UserLogin(LoginRequest userReq)
        {
            GenericApiResponse<LoginResponse> result = new GenericApiResponse<LoginResponse>();
            try
            {
                //check if the user is existing
                RegistrationModel details = await _registration.FetchRecord(m => m.Email == userReq.Email);

                if (details != null)
                {
                    string password = Decryptdata(details.Password);
                    if(userReq.Password == password)
                    {
                        LoginResponse loginResponse = new LoginResponse();
                        loginResponse.FirstName = details.FirstName;
                        loginResponse.LastName = details.LastName;
                        loginResponse.Email = details.Email;
                        loginResponse.Address = details.Address;
                        loginResponse.PhoneNumber = details.PhoneNumber;
                        loginResponse.DateCreated = details.DateCreated;
                        result.ResponseCode = ResponseCodes.Success;
                        result.ResponseDescription = "You login suceessfully.";
                        result.Data = loginResponse;

                    }
                     
                    else
                    {
                        result.ResponseDescription = "Unable to login successfully, Make sure you are using the correct email and password. Thank you.";
                        result.ResponseCode = ResponseCodes.Failure;

                    }                 

                }
                else
                {
                    result.ResponseDescription = "This user does not exist, kindly registerd before you login. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<IEnumerable<LibraryBook>>> GetAvaliableBooks()
        {
            GenericApiResponse<IEnumerable<LibraryBook>> result = new GenericApiResponse<IEnumerable<LibraryBook>>();
            try
            {
                //check and fetch only available books
                IEnumerable<LibraryBook> details = await _librarybooks.GetAll(m => m.StatusCode == 1);
                if (details != null)
                {
                        result.ResponseCode = ResponseCodes.Success;
                        result.ResponseDescription = "Avaliable books fetched successfully.";
                        result.Data = details; 
                }
                else
                {
                    result.ResponseDescription = "This user does not exist, kindly registerd before you login. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<BookOrder>> ReserveBook(ReservedBookRequest userReq)
        {
            GenericApiResponse<BookOrder> result = new GenericApiResponse<BookOrder>();
            try
            {
               // await _librarybooks.Add(library);
                //check and fetch the book requested
                LibraryBook bookrequested = await _librarybooks.FetchRecord(m => m.BookName == userReq.BookName && m.Author == userReq.Author && m.StatusCode == 1);

                if (bookrequested != null)
                {
                    //reserver the book for the user
                    BookOrder book = new BookOrder();
                    book.FirstName = userReq.FirstName;
                    book.LastName = userReq.LastName;
                    book.Address = userReq.Address;
                    book.PhoneNumber = userReq.PhoneNumber;
                    book.Email = userReq.Email;
                    book.BookName = userReq.BookName;
                    book.BookingStatusDescription = "Reserved";
                    book.DateCreated = DateTime.Now;
                    

                    await _bookOrder.Add(book);

                    //update the library books table and set avaliable to reserved
                    bookrequested.StatusCode = 2;// change status code to 2 to indicate that the book is reserved
                    bookrequested.Description = "Reserved";
                    bookrequested.BookReservedDate = DateTime.Now;
                    bookrequested.BookReservedBy = userReq.Email;

                    await _librarybooks.Update(bookrequested);
               
                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = "You have successfully reserved a book and you have within 24hrs to pick it up before the reserved time expires. Thank you.";
                    result.Data = null;
                }
                else
                {
                    result.ResponseDescription = "This book is currently not avaliable, kindly check for another book. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;
                    result.Data = null;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<BookOrder>> BorrowBook(BorrowBookRequest userReq)
        {
            GenericApiResponse<BookOrder> result = new GenericApiResponse<BookOrder>();
            try
            {
                
                //check if the requested  book is still reserved for the customer
                LibraryBook bookrequested = await _librarybooks.FetchRecord(m => m.BookName == userReq.BookName && m.BookReservedBy == userReq.Email && m.StatusCode == 2);

                //fetch the customers's order to update
                BookOrder borrowBook = await _bookOrder.FetchRecord(m => m.BookName == userReq.BookName && m.Email == userReq.Email && m.BookingStatusDescription == "Reserved");
                if (bookrequested != null && borrowBook != null)
                {
                    //The customer will return the book after 5 days
                    DateTime returnDate = DateTime.Today.AddDays(5);
                    borrowBook.BookingStatusDescription = "Borrowed";
                    borrowBook.BookReturnDate = returnDate;
                    borrowBook.BookBorrowedDate = DateTime.Now;

                    await _bookOrder.Update(borrowBook);

                    //update the library books table and set book to borrowed to reserved
                    //change the status code to 0 to indicate that the book is not available
                    bookrequested.StatusCode = 0;
                    bookrequested.Description = "Borrowed";
                    bookrequested.BookBorrowedDate = DateTime.Now;
                    bookrequested.BookReturnedDate = returnDate;

                    await _librarybooks.Update(bookrequested);

                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = $"You have successfully borrowed a book and you will return the book on or before {bookrequested.BookReturnedDate}. Thank you.";
                    result.Data = null;
                }
                else
                {
                    result.ResponseDescription = "This book is currently not avaliable, kindly check for another book. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;
                    result.Data = null;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<BookOrder>> ReturnBook(BorrowBookRequest userReq)
        {
            GenericApiResponse<BookOrder> result = new GenericApiResponse<BookOrder>();
            try
            {

                //fetch the book the customer wants to return
                LibraryBook bookrequested = await _librarybooks.FetchRecord(m => m.BookName == userReq.BookName && m.BookReservedBy == userReq.Email && m.StatusCode == 0);

                //fetch the customers's order to update
                BookOrder borrowBook = await _bookOrder.FetchRecord(m => m.BookName == userReq.BookName && m.Email == userReq.Email && m.BookingStatusDescription == "Borrowed");
                if (bookrequested != null && borrowBook != null)
                {
                 
                    borrowBook.BookingStatusDescription = "Returned";
                    borrowBook.BookReturnDate = DateTime.Now;

                    await _bookOrder.Update(borrowBook);

                    //update the library books table and set book from borrowed to available
                    //change the status code to 1 to indicate that the book is now available
                    bookrequested.StatusCode = 0;
                    bookrequested.Description = "Available";
                    bookrequested.BookReturnedDate = DateTime.Now;

                    await _librarybooks.Update(bookrequested);

                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = "You have successfully returned a book. Thank you.";
                    result.Data = null;
                }
                else
                {
                    result.ResponseDescription = "This book is not found, kindly try again. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;
                    result.Data = null;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }

        public async Task<GenericApiResponse<BookOrder>> ResetLibraryBookStatus()
        {
            GenericApiResponse<BookOrder> result = new GenericApiResponse<BookOrder>();
            try
            {

                //fetch the books that their reserved time is upto 24hrs
                LibraryBook bookrequested = await _librarybooks.FetchRecord(m => m.BookReservedDate == DateTime.Now.AddDays(-1) && m.StatusCode == 1);

                if (bookrequested != null)
                {

                    //update the library books table and set book from borrowed to available
                    //change the status code to 1 to indicate that the book is now available
                    DateTime? date = null;
                    bookrequested.StatusCode = 1;
                    bookrequested.Description = "Available";
                    bookrequested.BookReservedDate = date ;

                    await _librarybooks.Update(bookrequested);

                    result.ResponseCode = ResponseCodes.Success;
                    result.ResponseDescription = "You have successfully reset book status to avaliable. Thank you.";
                    result.Data = null;
                }
                else
                {
                    result.ResponseDescription = "No data is found. Thank you.";
                    result.ResponseCode = ResponseCodes.Failure;
                    result.Data = null;

                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = ResponseCodes.Failure;
                result.ResponseDescription = ex.Message.ToString();
                result.Data = null;
            }
            return result;
        }
        public string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

     
    }
}
