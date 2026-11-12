using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Diagnostics;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _api;

        public HomeController(ILogger<HomeController> logger, ApiService api)
        {
            _logger = logger;
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _api.GetAsync("api/Events");
                if (result.success)
                {
                    var events = JsonSerializer.Deserialize<List<EventViewModel>>(result.content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (events != null)
                        return View(events);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load events");
            }

            return View(new List<EventViewModel>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}