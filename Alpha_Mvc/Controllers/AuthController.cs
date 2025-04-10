using Alpha_Mvc.Models;
using Business.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers;

public class AuthController(IAuthService authService, IMemberUserService memberUserService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly IMemberUserService _memberUserService = memberUserService;

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInFormModel form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var formData = form.MapTo<SignInFormData>();

        var result = await _authService.SignInAsync(formData);
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

        //if (await _memberUserService.ExistsAsync(form.Email))
        //{
        //    ModelState.AddModelError("Exists", "User already exists.");
        //    return View(form);
        //}

        var newUser = form.MapTo<CreateUserFormData>();

        var result = await _memberUserService.CreateUserAsync(newUser);

        switch (result.StatusCode)
        {
            case 201:
                return RedirectToAction("SignIn");

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
