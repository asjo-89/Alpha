using Alpha_Mvc.Models;
using Business.Dtos;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Alpha_Mvc.Controllers;

public class AuthController(AuthService authService, SignInManager<MemberUserEntity> signInManager) : Controller
{
    private readonly AuthService _authService = authService;
    private readonly SignInManager<MemberUserEntity> _signInManager = signInManager;

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInFormModel form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, true, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid email och password. Please try again.");
        return View(form);
    }

    public IActionResult CreateAccount()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountModel form)
    {
        if (!ModelState.IsValid) 
            return View(form);

        if (await _authService.ExistsAsync(form.Email))
        {
            ModelState.AddModelError("Exists", "User already exists.");
            return View(form);
        }

        var user = new CreateAccountRegForm()
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            Password = form.Password
        };

        var result = await _authService.CreateAsync(user);

        switch (result)
        {
            case 201:
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
