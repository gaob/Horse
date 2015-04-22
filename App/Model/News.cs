using System;
using System.ComponentModel;

namespace App
{
	public class News : BaseViewModel
	{
		public News ()
		{
		}

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
	}
}
