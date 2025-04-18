using Alpha_Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Alpha_Mvc.ViewModels;

public class EditMemberViewModel
{
    public MemberUserModel Member { get; set; } = new();
    public IEnumerable<SelectListItem> Roles { get; set; } = [];
}
