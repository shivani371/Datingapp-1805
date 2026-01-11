using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
#nullable disable
public class RegisterDto
{
    [Required]
    public string DisplayName{get; set;}

[Required]
[EmailAddress]
    public string Email{get; set;}

[Required]
[MinLength(3)]
    public string PassWord{get; set;}

}
