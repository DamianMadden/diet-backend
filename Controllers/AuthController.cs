using draft_ml.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static draft_ml.Services.AuthProviderService;

[ApiController]
[AllowAnonymous]
[Route("auth")]
public class AuthController() : ControllerBase
{
    [HttpPost("reresh")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var tokenService = HttpContext.RequestServices.GetRequiredService<TokenService>();

        try
        {
            // Lookup sessions
            var userId = await tokenService.ValidateRefreshToken(request.RefreshToken);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Create new session token
            var tokenResponse = tokenService.CreateTokens((Guid)userId);

            return Ok(tokenResponse);
        }
        catch (Exception ex)
        {
            // General exception
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    [HttpPost("google")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleAuthRequest request)
    {
        var googleService = HttpContext.RequestServices.GetRequiredService<GoogleService>();

        try
        {
            var result = await googleService.SignIn(request.IdToken);

            switch (result.Status)
            {
                case SignInStatus.OK:
                    return Ok(result.TokenResponse);
                default:
                    // TODO: Handle everything
                    return BadRequest();
            }
        }
        catch (Exception ex)
        {
            // General exception
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }
}
