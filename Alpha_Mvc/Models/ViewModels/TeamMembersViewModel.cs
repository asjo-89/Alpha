namespace Alpha_Mvc.Models.ViewModels;

public class TeamMembersViewModel
{
    //public IEnumerable<UserModel> Users { get; set; } = [];
    public IEnumerable<UserModel> Users { get; set; } = [];

    public UserModel User { get; set; } = new();
    public CreateMemberFormModel Member { get; set; } = new();
}
