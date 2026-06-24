using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Core.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Ad boş olamaz.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter olmalı.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad boş olamaz.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter olmalı.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail boş olamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-mail giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre boş olamaz.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalı.")]
        public string Password { get; set; } = string.Empty;
    }
}
