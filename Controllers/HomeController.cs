using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PiApp.Models;

namespace PiApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["NumDigits"] = 10;
        return View();
    }

	[HttpPost]
	public async Task<IActionResult> Pi(int digits) {
		Console.WriteLine($"Received post request, digits={digits}, API response {ViewData["Digits"]}");
		using (var client = new HttpClient()) {
			client.BaseAddress = new Uri("http://localhost:5206");
			var response = await client.GetStringAsync($"pi?digits={digits}");
			if (response == null) {
				ViewData["Digits"] = "No API response";
			}
			else {
				ViewData["Digits"] = response;
			}
            //Console.WriteLine($"Received post request, digits={digits}, API response {ViewData["Digits"]}")
            ViewData["NumDigits"] = digits;
			return View("Index");
		}
	}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
