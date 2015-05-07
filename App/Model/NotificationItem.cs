using System;
using Newtonsoft.Json.Linq;

namespace App
{
	public class NotificationItem : BaseViewModel
	{
		public NotificationItem ()
		{
		}

		public string Id;

		private string user_id = string.Empty;

		public string User_id
		{
			get { return user_id; }
			set { SetProperty (ref user_id, value, "User_id");}
		}

		private string text = string.Empty;

		public string Text
		{
			get { return text; }
			set { SetProperty (ref text, value, "Text");}
		}

		public DateTime Time { get; set; }

		public NotificationItem(JObject theObject)
		{
			Id = theObject.Value<string> ("id");
			user_id = theObject.Value<string> ("user_id");
			text = theObject.Value<string> ("text");
			Time = DateTime.Parse (theObject.Value<string> ("time"));
		}
	}
}
