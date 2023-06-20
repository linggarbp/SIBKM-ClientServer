using API.Models;
using Client.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Authorize]
public class UniversityController : Controller
{
    private readonly UniversityRepository repository;

    public UniversityController(UniversityRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var results = await repository.Get();
        var universities = new List<University>();

        if (results != null)
        {
            universities = results.Data.ToList();
        }

        return View(universities);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(University university)
    {

        var result = await repository.Post(university);
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
        var Results = await repository.Get(id);
        //var universities = new University();

        //if (Results != null)
        //{
        //    universities = Results.Data;
        //}

        return View(Results.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var Results = await repository.Get(id);
        var universities = new University();

        if (Results.Data?.Id is null)
        {
            return View(universities);
        }
        else
        {
            universities.Id = Results.Data.Id;
            universities.Name = Results.Data.Name;
        }
        return View(universities);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(University university)
    {
        if (ModelState.IsValid)
        {
            var result = await repository.Put(university.Id, university);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (result.Code == 500)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await repository.Get(id);
        var university = result?.Data;

        return View(university);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int id)
    {
        var result = await repository.Delete(id);
        if (result.Code == 200)
        {
            TempData["Success"] = "Delete Data Success";
            return RedirectToAction(nameof(Index));
        }

        var university = await repository.Get(id);
        return View("Delete", university?.Data);
    }
}
