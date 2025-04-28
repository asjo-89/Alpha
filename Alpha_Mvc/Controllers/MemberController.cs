using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Mvc.Controllers;

public class MemberController(IMemberUserService memberUserService) : Controller
{
    private readonly IMemberUserService _memberUserService = memberUserService;

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<JsonResult> SearchMembers(string term)
    {
        if (string.IsNullOrEmpty(term))
            return Json(new List<object>());

        var members = await _memberUserService.GetMemberUsersAsync(term);

        return Json(members);
    }
}
