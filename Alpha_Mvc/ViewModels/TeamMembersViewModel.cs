using Alpha_Mvc.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alpha_Mvc.ViewModels;

public class TeamMembersViewModel
{
    public IEnumerable<MemberUserModel> Users { get; set; } = [];
    public List<SelectListItem> Roles { get; set; } = [];

    public MemberFormModel Member { get; set; } = new();
}
