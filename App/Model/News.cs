using System;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace App
{
	public class News : BaseViewModel
	{
		public News ()
		{
		}

		private string id;

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

		private string author_pic_url = string.Empty;

		public string Author_pic_url
		{
			get { return author_pic_url; }
			set { SetProperty (ref author_pic_url, value, "Author_pic_url");}
		}

		private string text = string.Empty;

		public string Text
		{
			get { return text; }
			set { SetProperty (ref text, value, "Text");}
		}

		private string pic_url = string.Empty;

		public string Pic_url
		{
			get { return pic_url; }
			set { SetProperty (ref pic_url, value, "Pic_url");}
		}

		//Internal field
		public DateTime PublishTime { get; set; }

		public string Horse_id { get; set; }

		public News(JObject theObject)
		{
			id = theObject.Value<string> ("id");
			author_id = theObject.Value<string> ("author_id");
			author_name = theObject.Value<string> ("author_name");
			author_pic_url = theObject.Value<string> ("author_pic_url");
			text = theObject.Value<string> ("text");
			pic_url = theObject.Value<string> ("pic_url");
			PublishTime = DateTime.Parse (theObject.Value<string> ("publishtime"));
			Horse_id = theObject.Value<string> ("horse_id");
		}

		public JToken ToJToken()
		{
			return JObject.FromObject (new { author_id = Author_id, 
											 author_name = Author_name,
											 author_pic_url = Author_pic_url,
											 text = Text,
											 pic_url = Pic_url,
											 publishtime = PublishTime.ToString (),
											 horse_id = Horse_id});
		}
	}
}
