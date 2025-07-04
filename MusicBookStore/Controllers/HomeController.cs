using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicBookStore.Models;

namespace MusicBookStore.Controllers;

public class HomeController : Controller
{
    private readonly IEmailService emailService;

    public HomeController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View();
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
