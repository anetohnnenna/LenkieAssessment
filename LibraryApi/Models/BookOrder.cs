using LibraryApi.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models
{
    public class BookOrder : BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string BookingStatusDescription { get; set; }
        public DateTime? BookBorrowedDate { get; set; }
        public DateTime? BookReturnDate { get; set; }
        public DateTime DateCreated { get; set; } 

        //public LibraryBook LibraryBook { get; set; }
    }
}
