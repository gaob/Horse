using System;

namespace App
{
	public class Comment : BaseViewModel
	{
		public Comment ()
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

		private string text = string.Empty;

		public string Text
		{
			get { return text; }
			set { SetProperty (ref text, value, "Text");}
		}

		public DateTime PublishTime { get; set; }
		public string NewsID { get; set; }
		public bool Liked { get; set; }
	}
}
