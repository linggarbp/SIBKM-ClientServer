using API.Models;
using API.ViewModels;
using Client.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;
public class AccountController : Controller
{
    private readonly AccountRepository repository;

    public AccountController(AccountRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM login)
    {
        var result = await repository.Login(login);
        if (result.Code == 200)
        {
            HttpContext.Session.SetString("JWToken", result.Data);
            return RedirectToAction("index", "home");
        }
        return View();
    }

    [HttpGet("/Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Redirect("/Account/Login");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM register)
    {
        var result = await repository.Register(register);
        if (result.Code == 200)
        {
            TempData["Success"] = "Insert Data Success";
            return RedirectToAction("/Account/Login");
        }
        else if (result.Code == 409)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        return View();
    }

    public async Task<IActionResult> Index()
    {
        //localhost/university/
        var results = await repository.Get();
        var accounts = new List<Account>();

        if (results != null)
        {
            accounts = results.Data.ToList();
        }

        return View(accounts);
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(Account account)
    {

        var result = await repository.Post(account);
        if (result.Code == 200)
        {
            TempData["Success"] = "Insert Data Success";
            return RedirectToAction(nameof(Index));
        }
        else if (result.Code == 409)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        //localhost/university/
        var results = await repository.Get(id);
        //var universities = new University();

        //if (Results != null)
        //{
        //    universities = Results.Data;
        //}

        return View(results.Data);
    }
}