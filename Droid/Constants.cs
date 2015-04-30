namespace App.Droid
{
    public static class Constants
    {
        // Google project number
        public const string SenderID = "332029287570"; 

        // Azure app specific connection string and hub path
        public const string ConnectionString = "Endpoint=sb://csci-e64-notification-01-net-eduhub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=y3joZk7LLf7uaKbqeO5oWHETy3v8pQQziByn0Q4M6YQ=";
        public const string NotificationHubPath = "csci-e64-notification-01-net-eduhub";
    }
}
