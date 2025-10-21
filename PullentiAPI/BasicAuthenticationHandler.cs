using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using PullentiAPI.Controllers;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace PullentiAPI
{

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
    }

    public class UserService : IUserService
    {
        public async Task<User> Authenticate(string username, string password)
        {
            // Implement your logic to validate username and password
            // e.g., against a database, a hardcoded list, etc.

            AppConfig appCfg = ApplicationConfiguration.GetConfig();

            if (username == appCfg.apiUser && password == appCfg.apiPassword)
            {
                return new User { Username = username }; // Return a User object
            }
            return null;
        }
    }

    public class User
    {
        public string Username { get; set; }
        // Other user properties
    }


    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService; // Your service to validate credentials

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService) // Inject your user service
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                // Validate credentials using your user service
                var user = await _userService.Authenticate(username, password);

                if (user == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }

                var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                // Add other claims as needed, e.g., roles
            };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
