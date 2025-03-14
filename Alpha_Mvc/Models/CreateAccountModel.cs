﻿using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class CreateAccountModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [RegularExpression(@"^[A-ZÅÄÖa-zåäö][a-zåäö'-]{1,49}$", 
        ErrorMessage = "First name must be at least one character long.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [RegularExpression(@"^[A-ZÅÄÖa-zåäö][a-zåäö'-]{1,49}$",
        ErrorMessage = "Last name must be at least one charachter long.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*-]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain: " +
        "one lower case and one upper case letter, one digit, one special character.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords does not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "You need to accept the terms.")]
    public bool Terms { get; set; } = false;
}
