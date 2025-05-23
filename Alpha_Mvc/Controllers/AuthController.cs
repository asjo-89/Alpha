﻿
using Alpha_Mvc.Factories;
using Alpha_Mvc.Models;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers;

public class AuthController(IAuthService authService, IPictureService pictureService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInFormModel form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var dto = AccountFactoryMCV.SignInDtoFromModel(form);

        var result = await _authService.SignInAsync(dto);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid email och password. Please try again.");
        return View(form);
    }


    [HttpPost]
    public async Task<IActionResult> SignOut(MemberUserModel member)
    {
        var dto = MemberUserFactoryMCV.CreateDtoFromModel(member);
        await _authService.SignOutAsync(dto);

        return View("SignIn");
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

        var dto = AccountFactoryMCV.CreateDtoFromModel(form);

        var result = await _authService.CreateUserAsync(dto);

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
