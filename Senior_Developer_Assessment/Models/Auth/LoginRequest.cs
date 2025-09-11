// Models/Auth/LoginRequest.cs

namespace Senior_Developer_Assessment.Models.Auth
{

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}