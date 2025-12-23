using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using draft_ml.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace draft_ml.Services;

public class TokenService
{
    private readonly AuthOptions opt;
    private readonly ILogger<AuthProviderService> logger;
    private readonly IServiceScopeFactory scopeFactory;

    private readonly RandomNumberGenerator random = RandomNumberGenerator.Create();
    private readonly Lock mutex = new();

    private readonly SHA256 sha = SHA256.Create();

    public TokenService(
        IOptionsMonitor<AuthOptions> authOptMon,
        ILogger<AuthProviderService> _logger,
        IServiceScopeFactory _scopeFactory
    )
    {
        opt = authOptMon.CurrentValue;
        logger = _logger;
        scopeFactory = _scopeFactory;
    }

    public async Task<TokenResponse> CreateTokens(Guid userId)
    {
        var accessExpiry = DateTime.UtcNow.AddMinutes(opt.AccessTokenLifetimeMinutes);
        var accessToken = CreateSessionToken(userId, accessExpiry);
        var refreshToken = await CreateRefreshToken(userId);

        return new TokenResponse
        {
            AccessToken = accessToken,
            ExpiresIn = opt.AccessTokenLifetimeMinutes * 60,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Scope = string.Empty,
        };
    }

    public string CreateSessionToken(Guid userId, DateTime expiry)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(opt.SigningKey);
        try
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "http://10.0.2.2:5180",
                Audience = "http://10.0.2.2:5180",
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
                Expires = expiry,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Claims = new Dictionary<string, object> { },
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating token");
            return string.Empty;
        }
    }

    public async Task<string> CreateRefreshToken(Guid userId)
    {
        // Generate refresh token
        var refreshToken = GenerateCryptoRandomToken();

        // Add refresh token to db
        using var scope = scopeFactory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DietDbContext>();
        dbContext.Sessions.Add(
            new Session
            {
                UserId = userId,
                RefreshTokenHash = CalculateHash(refreshToken),
                Expiry = DateTime.UtcNow.AddDays(opt.RefreshTokenLifetimeDays),
            }
        );

        await dbContext.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<Guid?> ValidateRefreshToken(string refreshToken)
    {
        // Calculate token hash
        var refreshTokenHash = CalculateHash(refreshToken);

        // Check db for the existing refresh token
        using var scope = scopeFactory.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DietDbContext>();
        return (
            await dbContext.Sessions.FirstOrDefaultAsync<Session>(s =>
                s.RefreshTokenHash == refreshTokenHash
            )
        )?.UserId;
    }

    private string GenerateCryptoRandomToken()
    {
        using var _lock = mutex.EnterScope();

        byte[] randomBytes = new byte[40];
        random.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private string CalculateHash(string refreshToken)
    {
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(refreshToken)));
    }
}
