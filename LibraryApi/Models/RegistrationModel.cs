using System.ComponentModel.DataAnnotations;
using LibraryApi.Entities;

namespace LibraryApi.Models
{
    public class RegistrationModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string Address { get; set; } 
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
