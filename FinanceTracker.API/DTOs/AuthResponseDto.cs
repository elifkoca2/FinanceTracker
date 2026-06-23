namespace FinanceTracker.API.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class RegisterResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Message { get; set; } = "Kayıt başarılı. Giriş Yapabilirsiniz.";
    }
}
