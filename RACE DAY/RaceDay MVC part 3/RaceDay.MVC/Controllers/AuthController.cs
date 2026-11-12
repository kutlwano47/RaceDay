using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace RaceDay.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _api;
        public AuthController(ApiService api) { _api = api; }

        [HttpGet] public IActionResult Register() => View();
        [HttpGet] public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var (success, content) = await _api.PostAsync("/api/Auth/register", model);
            if (success)
            {
                TempData["Success"] = "Registered! Please login.";
                return RedirectToAction("Login");
            }
            ViewBag.Error = content;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var (success, content) = await _api.PostAsync("/api/Auth/login", model);
            if (!success)
            {
                ViewBag.Error = content;
                return View(model);
            }
            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                string token = "";
                if (root.TryGetProperty("token", out var t) && t.ValueKind != JsonValueKind.Null) token = t.GetString();
                else if (root.TryGetProperty("Token", out var t2) && t2.ValueKind != JsonValueKind.Null) token = t2.GetString();
                else if (root.TryGetProperty("accessToken", out var t3) && t3.ValueKind != JsonValueKind.Null) token = t3.GetString();
                else if (root.TryGetProperty("access_token", out var t4) && t4.ValueKind != JsonValueKind.Null) token = t4.GetString();
                else if (root.TryGetProperty("jwt", out var t5) && t5.ValueKind != JsonValueKind.Null) token = t5.GetString();
                else if (root.TryGetProperty("result", out var rObj) && rObj.ValueKind == JsonValueKind.Object)
                {
                    if (rObj.TryGetProperty("token", out var rt)) token = rt.GetString();
                }

                string role = "Athlete";
                if (root.TryGetProperty("role", out var r)) role = r.GetString() ?? "Athlete";
                else if (root.TryGetProperty("Role", out var r2)) role = r2.GetString() ?? "Athlete";

                if (string.IsNullOrWhiteSpace(token))
                {
                    ViewBag.Error = $"Login OK but token field not found. API returned: {content}";
                    return View(model);
                }

                // SAVE IN SESSION - BOTH NAMES
                HttpContext.Session.SetString("JWT", token);
                HttpContext.Session.SetString("JwtToken", token);
                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", model.Email);

                // SAVE IN CLAIMS TOO - SO ORGANISER CAN READ EVEN IF SESSION FAILS
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("JWT", token),
                    new Claim("JwtToken", token),
                    new Claim("Token", token),
                    new Claim("access_token", token)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Token read failed: " + content + " | Error: " + ex.Message;
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}