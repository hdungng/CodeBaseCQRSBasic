using CodeBaseCQRSBasic.Domain;
using CodeBaseCQRSBasic.DTOs.Auth;
using CodeBaseCQRSBasic.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseCQRSBasic.Services;

public class AuthService(
    AppDbContext context,
    ITokenService tokenService,
    IPasswordHasherService passwordHasher,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            logger.LogWarning("Login failed for username {UserName}.", request.UserName);
            return null;
        }

        var response = await IssueTokensAsync(user, cancellationToken);
        logger.LogInformation("Login successful for user {UserId}.", user.Id);

        return response;
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var hashedToken = passwordHasher.Hash(refreshToken);

        var storedToken = await context.RefreshTokens
            .Include(x => x.User)
            .ThenInclude(x => x!.Role)
            .SingleOrDefaultAsync(x => x.Token == hashedToken, cancellationToken);

        if (storedToken is null)
        {
            logger.LogWarning("Token refresh failed because refresh token does not exist.");
            return null;
        }

        if (storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.UtcNow)
        {
            logger.LogWarning("Token refresh rejected for user {UserId}. Revoked: {IsRevoked}, Expired: {IsExpired}.",
                storedToken.UserId,
                storedToken.IsRevoked,
                storedToken.ExpiryDate <= DateTime.UtcNow);
            return null;
        }

        storedToken.IsRevoked = true;

        var response = await IssueTokensAsync(storedToken.User!, cancellationToken);

        logger.LogInformation("Token refresh successful for user {UserId}.", storedToken.UserId);

        return response;
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var hashedToken = passwordHasher.Hash(refreshToken);

        var storedToken = await context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == hashedToken, cancellationToken);

        if (storedToken is null)
        {
            throw new KeyNotFoundException("Refresh token was not found.");
        }

        if (storedToken.IsRevoked)
        {
            return false;
        }

        storedToken.IsRevoked = true;
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Refresh token revoked for user {UserId}.", storedToken.UserId);
        return true;
    }

    private async Task<TokenResponse> IssueTokensAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var refreshExpiry = tokenService.GetRefreshTokenExpiryUtc();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = passwordHasher.Hash(refreshToken),
            UserId = user.Id,
            ExpiryDate = refreshExpiry,
            IsRevoked = false
        });

        await context.SaveChangesAsync(cancellationToken);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = tokenService.GetAccessTokenExpiryUtc(),
            RefreshTokenExpiresAtUtc = refreshExpiry
        };
    }
}
