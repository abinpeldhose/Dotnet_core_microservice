namespace customerSdmodule.ModelClass.Notifications
{
    public class NotificationRequest : Request
    {

        public NotificationRequest()
        {
            NotificationRequest._Requesttype = "Notificationrequest";
        }

        public NotificationApi Data { get => (NotificationApi)base.Data; set => base.Data = (NotificationApi)value; }
    }
}
