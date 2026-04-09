namespace BioDomes.Infrastructures.Models;

public class UserEntity
{
    public int Id { get; set; } 
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public DateOnly Birthdate { get; set; }
    
    public string SearchOrganism { get; set; }
    
    public string? ImageUrl { get; set; }
}