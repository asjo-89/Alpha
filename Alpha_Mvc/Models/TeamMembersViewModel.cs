namespace Alpha_Mvc.Models;

public class TeamMembersViewModel
{
    public IEnumerable<UserModel> Users { get; set; } = [];

    public UserModel User { get; set; } = new();
    public CreateMemberFormModel Member { get; set; } = new();
}
