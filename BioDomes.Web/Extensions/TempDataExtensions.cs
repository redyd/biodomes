using BioDomes.Web.InputModel;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BioDomes.Web.Extensions;

public static class TempDataExtensions
{
    private static readonly string Key = "Notification";

    public static void PushNotification(this ITempDataDictionary tempData, Notification notification)
    {
        tempData[$"{Key}.Message"] = notification.Message;
        tempData[$"{Key}.Type"] = notification.Type.ToString();
    }

    public static Notification? ConsumeNotification(this ITempDataDictionary tempData)
    {
        var rawMsg = tempData[$"{Key}.Message"];
        var rawType = tempData[$"{Key}.Type"];
        
        if (rawMsg is null || rawType is null) return null;
        if (rawMsg is not string message || rawType is not string stringType) return  null;
        
        var type = (NotificationType)Enum.Parse(typeof(NotificationType), stringType);
        return new Notification
        {
            Message = message,
            Type = type
        };
    }
}