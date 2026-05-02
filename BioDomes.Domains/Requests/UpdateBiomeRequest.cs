namespace BioDomes.Domains.Requests;

public class UpdateBiomeRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsPublic { get; set; }
}