namespace BioDomes.Web.ViewModels;

public class EquipmentView
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required string Produced { get; set; }
    public required string Consumed { get; set; }
}