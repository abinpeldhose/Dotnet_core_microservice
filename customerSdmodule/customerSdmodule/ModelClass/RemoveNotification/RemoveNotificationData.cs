namespace customerSdmodule.ModelClass.RemoveNotification
{
    public class RemoveNotificationData:BaseData
    {
        private string _userid;
        private int _alertId;

        public string userId { get => _userid; set => _userid = value; }
        public int alertId { get => _alertId; set => _alertId = value; }
    }
}
