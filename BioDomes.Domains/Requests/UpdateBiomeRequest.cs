namespace BioDomes.Domains.Requests;

public class UpdateBiomeRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public bool? IsPublic { get; set; }
}