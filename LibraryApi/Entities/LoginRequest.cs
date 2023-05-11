namespace LibraryApi.Entities
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Scope { get; set; }
    }
}
