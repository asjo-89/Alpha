using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class StatusModel
{
    public Guid Id { get; set; }


    [Required(ErrorMessage = "Status name is required...")]
    [Display(Name = "Status Name", Prompt = "Enter status name...")]
    public string StatusName { get; set; } = null!;
}
