﻿using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class SignInFormModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "You need to enter your email address.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "You need to enter your password.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*-]).{8,}$")]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; }
}
