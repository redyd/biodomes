using Microsoft.AspNetCore.Identity;

namespace BioDomes.Infrastructures.Models;

public class UserEntity : IdentityUser
{
    public DateOnly? Birthdate { get; set; }
    
    public string? SearchOrganism { get; set; }
    
    public string? ImageUrl { get; set; }
}