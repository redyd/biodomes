namespace BioDomes.Domains.Core;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
    public string SearchOrganism { get; set; }
    public string? ImageUrl { get; set; }

}