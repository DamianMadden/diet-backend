using System.Text.Json;
using draft_ml.Db;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace draft_ml.Services;

public class GoogleService : AuthProviderService
{
    private readonly TokenService tokenService;
    private readonly AuthOptions opt;
    private readonly ILogger<AuthProviderService> logger;

    public GoogleService(
        TokenService _tokenService,
        IOptionsMonitor<AuthOptions> authOptMon,
        ILogger<AuthProviderService> _logger,
        DietDbContext db
    )
        : base(db, _logger)
    {
        tokenService = _tokenService;
        opt = authOptMon.CurrentValue;
        logger = _logger;
    }

    public override async Task<SignInResult> SignIn(string idToken)
    {
        // Validate the ID token to get user information
        var validationSettings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [opt.Google.ClientId],
        };

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, validationSettings);

            var userId = await FindOrCreateUser(
                payload.Subject,
                payload.Email,
                payload.GivenName,
                payload.FamilyName,
                payload.EmailVerified,
                IdentityProvider.GoogleIdentity
            );

            // Create JWT and refresh token
            var tokenResponse = await tokenService.CreateTokens(userId);

            return new() { TokenResponse = tokenResponse, Status = SignInStatus.OK };
        }
        catch (InvalidJwtException ex)
        {
            logger.LogError(ex, "Failed to validate ID token");

            return new() { Status = SignInStatus.TokenValidationFailure };
        }
    }
}
