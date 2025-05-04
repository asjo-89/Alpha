﻿using System.ComponentModel.DataAnnotations;

namespace Alpha_Mvc.Models;

public class MemberUserModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name", Prompt = "Enter first name...")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name", Prompt = "Enter last name...")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "Email is required.")]
    [Display(Name = "Email", Prompt = "Enter email address...")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]

    public string Email { get; set; } = null!;


    [Display(Name = "Phone number", Prompt = "Enter phone number...")]
    [RegularExpression(@"^(?:\+46|0)([ ]?)(\d{1,3})([ -]?)(\d{2,3})([ -]?)(\d{2})([ -]?)(\d{2})$", ErrorMessage = "Invalid phone number.")]

    public string? PhoneNumber { get; set; }


    [Required(ErrorMessage = "Job title is required.")]
    [Display(Name = "Job title", Prompt = "Enter job title...")]
    public string JobTitle { get; set; } = null!;


    [Required(ErrorMessage = "Street address is required.")]
    [Display(Name = "Street address", Prompt = "Enter street address...")]
    public string StreetAddress { get; set; } = null!;


    [Required(ErrorMessage = "Postal code is required.")]
    [Display(Name = "Postal Code", Prompt = "Enter postal code...")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Only digits allowed.")]
    public string PostalCode { get; set; } = null!;


    [Required(ErrorMessage = "City is required.")]
    [Display(Name = "City", Prompt = "Enter city...")]
    public string City { get; set; } = null!;


    public int? BirthDay { get; set; }
    public int? BirthMonth { get; set; }
    public int? BirthYear { get; set; }

    [Display(Name = "Date Of Birth", Prompt = "Enter date of birth...")]
    public DateOnly DateOfBirth { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; set; }

    [DataType(DataType.ImageUrl)]
    public IFormFile? ProfileImage { get; set; }
    public Guid? PictureId { get; set; }
    public Guid? RoleId { get; set; }
}
