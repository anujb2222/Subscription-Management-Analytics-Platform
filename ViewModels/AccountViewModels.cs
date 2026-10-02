using System.ComponentModel.DataAnnotations;

namespace Sublytic.ViewModels
{
public class RegisterViewModel
{
[Required]
[EmailAddress]
public string Email { get; set; }

    [Required]
    [RegularExpression(@"^\d{6,}$",
        ErrorMessage = "Password must be at least 6 numbers.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^\d{6,}$",
        ErrorMessage = "Password must be at least 6 numbers.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}


}