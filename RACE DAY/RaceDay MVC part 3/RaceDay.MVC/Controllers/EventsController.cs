using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public EventsController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        private HttpClient GetClient()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7181");
            var token = HttpContext.Session.GetString("token");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("token") == null)
                return RedirectToAction("Login", "Auth");

            var client = GetClient();
            try
            {
                var events = await client.GetFromJsonAsync<List<EventVM>>("/api/Events");
                return View(events);
            }
            catch
            {
                return View(new List<EventVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("token") == null)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventVM model, IFormFile? bannerFile)
        {
            if (HttpContext.Session.GetString("token") == null)
                return RedirectToAction("Login", "Auth");

            // IGNORE bannerFile for now - we removed BannerUrl from DB
            var client = GetClient();

            // Map MVC -> API names correctly
            var apiModel = new
            {
                EventName = model.Name,
                Description = model.Description,
                EventDate = model.Date,
                Location = model.Location,
                DistanceKM = model.Distance,
                EventType = model.Type,
                OrganiserID = 0 // API will auto-fill from token
            };

            var res = await client.PostAsJsonAsync("/api/Events", apiModel);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Index");

            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var err = await res.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"API Error: {err}");
            return View(model);
        }
    }

    public class EventVM
    {
        public int EventId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Location { get; set; } = "";
        public double Distance { get; set; }
        public string Type { get; set; } = "Run";
        public string? BannerUrl { get; set; }
    }

    public class UploadResult { public string url { get; set; } = ""; }
}