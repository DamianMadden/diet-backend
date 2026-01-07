using draft_ml.Data;
using draft_ml.Db;
using Microsoft.EntityFrameworkCore;

namespace draft_ml.Services;

public abstract class AuthProviderService(DietDbContext db, ILogger logger)
{
    public enum SignInStatus
    {
        OK = 0,
        AuthProviderResponseFailure = 1,
        AuthProviderConnectionFailure = 2,
        TokenValidationFailure = 3,
    };

    public class SignInResult
    {
        public SignInStatus Status { get; set; }
        public TokenResponse? TokenResponse { get; set; }
    }

    /// <summary>
    /// Signs in a user using an authorization code. If the user doesn't exist it should invoke the method to create one.
    /// </summary>
    /// <param name="code">The authorization code from the identity provider.</param>
    /// <returns>A status indicating the result of the sign-in attempt.</returns>
    public abstract Task<SignInResult> SignIn(string code);

    /// <summary>
    /// Returns a session for a user whether or not they exist. Creates the user if not present
    /// </summary>
    /// <param name="externalId">The user's identifier from the identity provider.</param>
    /// <param name="email">The user's email.</param>
    /// <param name="provider">Enumeration value for the identity provider.</param>
    /// <returns>A guid which identifies the user</returns>
    protected async Task<Guid> FindOrCreateUser(
        string externalId,
        string email,
        string givenName,
        string familyName,
        bool emailVerified,
        IdentityProvider provider
    )
    {
        try
        {
            // Find user with the external id
            var user = await db
                .UserExternalIdentities.Where(u =>
                    u.ExternalIdentity == externalId && u.IdentityProvider == provider
                )
                .Select(u => u.User)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    GivenName = givenName,
                    FamilyName = familyName,
                    EmailVerified = emailVerified,
                };

                db.Users.Add(user);

                db.UserExternalIdentities.Add(
                    new UserExternalIdentity
                    {
                        User = user,
                        UserId = user.Id,
                        ExternalIdentity = externalId,
                        IdentityProvider = provider,
                    }
                );

                await db.SaveChangesAsync();
            }
            else if (user.Email != email)
            {
                user.Email = email;
                user.GivenName = givenName;
                user.FamilyName = familyName;
                user.EmailVerified = emailVerified;

                await db.SaveChangesAsync();
            }

            return user.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to find or create user");
            throw;
        }
    }
}
