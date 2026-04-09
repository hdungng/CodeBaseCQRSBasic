using CodeBaseCQRSBasic.DTOs.Auth;

namespace CodeBaseCQRSBasic.Services;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken);
}
