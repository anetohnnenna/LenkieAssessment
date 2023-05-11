using LibraryApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class LibraryBook : BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string Author { get; set; }
        
        [Required]
        public int StatusCode { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string? BookReservedBy { get; set; }
        public DateTime? BookReservedDate { get; set; }
        public DateTime? BookBorrowedDate { get; set; }
        public DateTime? BookReturnedDate { get; set; }

        // public ICollection<BookingOrder> BookingOrders { get; set; }
    }
}
