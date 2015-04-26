using System;
using Newtonsoft.Json.Linq;

namespace App
{
	public class Comment : BaseViewModel
	{
		public Comment ()
		{
		}

		private string Id;

		private string author_id = string.Empty;

		public string Author_id
		{
			get { return author_id; }
			set { SetProperty (ref author_id, value, "Author_id");}
		}

		private string author_name = string.Empty;

		public string Author_name
		{
			get { return author_name; }
			set { SetProperty (ref author_name, value, "Author_name");}
		}

		private string text = string.Empty;

		public string Text
		{
			get { return text; }
			set { SetProperty (ref text, value, "Text");}
		}

		public DateTime PublishTime { get; set; }
		public string NewsID { get; set; }
		public bool Liked { get; set; }

		public Comment(JObject theObject)
		{
			Id = theObject.Value<string> ("id");
			author_id = theObject.Value<string> ("author_id");
			author_name = theObject.Value<string> ("author_name");
			text = theObject.Value<string> ("text");
			PublishTime = DateTime.Parse (theObject.Value<string> ("publishtime"));
			NewsID = theObject.Value<string> ("news_id");
			Liked = bool.Parse (theObject.Value<string> ("liked"));
		}

		public JToken ToJToken()
		{
			return JObject.FromObject (new { author_id = Author_id, 
											 author_name = Author_name,
											 text = Text,
											 publishtime = PublishTime.ToString (),
											 news_id = NewsID,
											 liked = Liked ? "true" : "false"});
		}
	}
}
