using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Alpha_Mvc.Controllers;

public class AuthController(AuthService authService) : Controller
{
    private readonly AuthService _authService = authService;

    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult CreateAccount()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRegForm form)
    {
        if (!ModelState.IsValid) 
            return View(form);

        if (await _authService.ExistsAsync(form.EmailAddress))
        {
            ModelState.AddModelError("Exists", "User already exists.");
            return View(form);
        }

        var result = await _authService.CreateAsync(form);

        switch (result)
        {
            case 200:
                return RedirectToAction("SignIn", "Auth");

            case 400:
            {
                ModelState.AddModelError("Invalid form", "Required fields can not be empty..");
                return View(form);
            }

            default:
            {
                ModelState.AddModelError("Unexpected Error", "An unexpected error occured.");
                return View(form);
            }
        }
    }
}
