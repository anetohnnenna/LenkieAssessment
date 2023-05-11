using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Entities
{
    public class RegRequestModel
    {
        [Required] public string? FirstName { get; set; }
        [Required] public string? LastName { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? PhoneNumber { get; set; }
        [Required] public string? Address { get; set; }
        [Required] public string? Password { get; set; }
    }
}
