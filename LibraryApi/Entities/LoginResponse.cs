namespace LibraryApi.Entities
{
    public class LoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; }
        public string Access_Token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
    }
}
