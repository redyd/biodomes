namespace BioDomes.Web.InputModel;

public class Notification
{
    public required string Message { get; set; }
    public required NotificationType Type { get; set; }
}

public enum NotificationType
{
    Success,
    Error
}