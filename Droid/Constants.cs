namespace App.Droid
{
    public static class Constants
    {
        // Google project number
		public const string SenderID = "795457928950"; 

        // Azure app specific connection string and hub path
		public const string ConnectionString = "Endpoint=sb://dotnet3hub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=x2L2DwlRLBoOGr2hJjEleZ+K6pUmojKj6otelMtSGOc=";
		public const string NotificationHubPath = "dotnet3hub";
    }
}
