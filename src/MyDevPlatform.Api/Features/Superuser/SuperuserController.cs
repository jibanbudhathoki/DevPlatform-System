using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using MyDevPlatform.Application.Organizations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyDevPlatform.Api.Features.Superuser;

[Route("super-admin")]
[Authorize(Roles = "Superuser")]
[AutoValidateAntiforgeryToken]
public class SuperuserController : Controller
{
    private readonly OrganizationService _organizationService;
    private readonly ILogger<SuperuserController> _logger;
    private readonly IConfiguration _configuration;

    public SuperuserController(OrganizationService organizationService, ILogger<SuperuserController> logger, IConfiguration configuration)
    {
        _organizationService = organizationService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("")]
    [HttpGet("/")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await GetDashboardModel(cancellationToken);
        ViewData["Section"] = "Overview";
        return View("~/Views/superuser/Pages/Dashboard.cshtml", model);
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl = null)
    {
        var model = new SuperuserLoginViewModel { ReturnUrl = returnUrl };
        if (string.IsNullOrWhiteSpace(_configuration["Superuser:Email"]) || string.IsNullOrEmpty(_configuration["Superuser:Password"]))
        {
            model.ErrorMessage = "Superuser sign-in is not configured. Set SUPERUSER__EMAIL and SUPERUSER__PASSWORD.";
        }

        return View("~/Views/superuser/Pages/Login.cshtml", model);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(SuperuserLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/superuser/Pages/Login.cshtml", model);
        }

        var configuredEmail = _configuration["Superuser:Email"];
        var configuredPassword = _configuration["Superuser:Password"];
        if (string.IsNullOrWhiteSpace(configuredEmail) || string.IsNullOrEmpty(configuredPassword))
        {
            model.ErrorMessage = "Superuser sign-in is not configured. Set SUPERUSER__EMAIL and SUPERUSER__PASSWORD.";
            return View("~/Views/superuser/Pages/Login.cshtml", model);
        }

        var suppliedPassword = Encoding.UTF8.GetBytes(model.Password);
        var expectedPassword = Encoding.UTF8.GetBytes(configuredPassword);
        var passwordMatches = CryptographicOperations.FixedTimeEquals(suppliedPassword, expectedPassword);
        if (!string.Equals(model.Email, configuredEmail, StringComparison.OrdinalIgnoreCase) || !passwordMatches)
        {
            model.ErrorMessage = "Email or password is incorrect.";
            return View("~/Views/superuser/Pages/Login.cshtml", model);
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, configuredEmail),
            new Claim(ClaimTypes.Role, "Superuser")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return Url.IsLocalUrl(model.ReturnUrl)
            ? LocalRedirect(model.ReturnUrl!)
            : RedirectToAction(nameof(Index));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    private async Task<SuperuserDashboardViewModel> GetDashboardModel(CancellationToken cancellationToken)
    {
        try
        {
            var organizations = await _organizationService.GetOrganizationsAsync(cancellationToken);
            return new SuperuserDashboardViewModel(organizations, true, null);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Unable to load organizations for the Superuser dashboard.");
            var message = "Organization data is unavailable. Check the database connection and try again.";
            return new SuperuserDashboardViewModel([], false, message);
        }
    }
}

[Route("su")]
public class SuperuserRedirectController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => Redirect("/super-admin/");

    [HttpGet("organization")]
    public IActionResult Organization() => Redirect("/super-admin/organization");

    [HttpGet("users")]
    public IActionResult Users() => Redirect("/super-admin/users");

    [HttpGet("login")]
    public IActionResult Login() => Redirect("/super-admin/login");
}