using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using FinanceTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Başarısız giriş denemesi: {Email}", dto.Email);
                return null;
            }

            // UserManager şifreyi kendi hash'iyle karşılaştırıyor
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Başarısız giriş denemesi: {Email}", dto.Email);
                return null;
            }

            _logger.LogInformation("Kullanıcı giriş yaptı: {Email}", user.Email);

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                FirstName = user.FirstName,
                Email = user.Email!,
                ExpiresAt = _jwtService.GetExpirationDate()
            };

        }

        public async  Task<RegisterResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Kayıt denemesi başarısız, e-mail zaten kayıtlı: {Email}", dto.Email);
                throw new InvalidOperationException("Bu e-mail adresi zaten kayıtlı.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,      
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            // UserManager şifreyi otomatik hash'ler
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Kayıt başarısız: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }

            _logger.LogInformation("Yeni kullanıcı kaydedildi: {Email}", user.Email);

            var token = _jwtService.GenerateToken(user);

            return new RegisterResponseDto
            {
                 Email = user.Email,
                 FirstName = user.FirstName
            };
        }
    }
}
