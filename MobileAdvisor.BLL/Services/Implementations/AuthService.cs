using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MobileAdvisor.BLL.DTOs.Auth;
using MobileAdvisor.BLL.Services.Interfaces;
using MobileAdvisor.DAL.Entities;
using MobileAdvisor.DAL.Entities.Enums;
using MobileAdvisor.DAL.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobileAdvisor.BLL.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await _unitOfWork.Users.AnyAsync(u => u.Email == dto.Email))
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            PhoneNumber = dto.PhoneNumber,
            Role = dto.Role,
            Status = dto.Role == UserRole.Expert ? AccountStatus.PendingApproval : AccountStatus.Active
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        if (user.Role == UserRole.Expert)
        {
            var profile = new ExpertProfile
            {
                UserId = user.Id,
                Bio = "New expert profile pending setup",
                YearsOfExperience = 0,
                DefaultFee = 0,
                IsApproved = false,
                IsAvailable = false
            };
            await _unitOfWork.ExpertProfiles.AddAsync(profile);
            await _unitOfWork.CompleteAsync();
        }

        var token = GenerateJwtToken(user);
        return new AuthResponseDto(user.Id, user.FullName, user.Email, user.Role.ToString(), token);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        if (user.Status == AccountStatus.Suspended || user.Status == AccountStatus.Rejected)
        {
            throw new UnauthorizedAccessException("Account is not active.");
        }

        var token = GenerateJwtToken(user);
        return new AuthResponseDto(user.Id, user.FullName, user.Email, user.Role.ToString(), token);
    }

    private string GenerateJwtToken(User user)
    {
        var secret = _configuration["Jwt:Key"] ?? "MobileAdvisorSecretKeyLongerThan32BytesForHmacSha256!";
        var issuer = _configuration["Jwt:Issuer"] ?? "MobileAdvisorAPI";
        var audience = _configuration["Jwt:Audience"] ?? "MobileAdvisorClients";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
