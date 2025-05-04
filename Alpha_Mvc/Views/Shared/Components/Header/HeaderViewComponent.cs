using Alpha_Mvc.Factories;
using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Views.Shared.Components.Header;

public class HeaderViewComponent(IMemberUserService memberService) : ViewComponent
{
    private readonly IMemberUserService _memberService = memberService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var loggedInUser = await _memberService.GetLoggedInUserAsync();

        if (loggedInUser?.Data == null)
        {
            var user = new MemberUser
            {
                FirstName = "Unknown",
                LastName = "Unknown",
                Email = "Unknown",
                ImageUrl = "images/Profiles/Profile2.png"
            };

            return View(MemberUserFactoryMCV.CreateModelFromDomainModel(user));
        }
        var userModel = MemberUserFactoryMCV.CreateModelFromDomainModel(loggedInUser?.Data!);
        return View(userModel);
    }
}
