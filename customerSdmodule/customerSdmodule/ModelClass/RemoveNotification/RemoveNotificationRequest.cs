namespace customerSdmodule.ModelClass.RemoveNotification
{
    public class RemoveNotificationRequest:Request
    { 
     public RemoveNotificationRequest()
    {
            RemoveNotificationRequest._Requesttype = "RemoveNotificationRequest";
    }
    public RemoveNotificationApi Data { get => (RemoveNotificationApi)base.Data; set => base.Data = (RemoveNotificationApi)value; }

}
}
