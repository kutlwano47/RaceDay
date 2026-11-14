using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Security.Claims;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    [Authorize]
    public class OrganiserController : Controller
    {
        private readonly ApiService _api;
        public OrganiserController(ApiService api) => _api = api;

        private string GetToken()
        {
            return HttpContext.Session.GetString("JWT")
                ?? HttpContext.Session.GetString("JwtToken")
                ?? HttpContext.Session.GetString("Token")
                ?? User.FindFirstValue("JWT")
                ?? User.FindFirstValue("access_token")
                ?? "";
        }

        public IActionResult Create()
        {
            return View(new EventViewModel
            {
                Date = DateTime.Now.AddDays(7),
                Distance = 10,
                Location = "Polokwane",
                Name = "Polokwane City Run"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // FIXED PAYLOAD - Matches API exactly
            var payload = new
            {
                EventName = model.Name,
                Description = model.Description ?? "10KM fun run in Polokwane",
                Location = model.Location ?? "Polokwane",
                EventDate = model.Date.ToUniversalTime(),
                DistanceKM = model.Distance,
                EventType = string.IsNullOrWhiteSpace(model.EventType)
                    ? "Road"
                    : model.EventType == "Road Running" ? "Road" : model.EventType
            };

            var (success, content) = await _api.PostAsync("/api/Events", payload, token);

            if (success)
            {
                TempData["Success"] = $"Race '{model.Name}' created!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", $"API Error: {content}");
            return View(model);
        }

        public async Task<IActionResult> Events()
        {
            var token = GetToken();
            var (success, content) = await _api.GetAsync("/api/Events", token);
            var list = new List<EventViewModel>();
            if (success)
            {
                try
                {
                    list = JsonSerializer.Deserialize<List<EventViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                }
                catch { }
            }
            return View(list);
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = GetToken();
            var (success, content) = await _api.GetAsync("/api/Events", token);
            var events = new List<EventViewModel>();
            if (success)
            {
                try { events = JsonSerializer.Deserialize<List<EventViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new(); } catch { }
            }
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = GetToken();
            await _api.DeleteAsync($"/api/Events/{id}", token);
            return RedirectToAction("Events");
        }
    }
}