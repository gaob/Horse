using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace App
{
	public class NewsDetailsViewModel : BaseViewModel
	{
		private ObservableCollection<Comment> commentsItems = new ObservableCollection<Comment>();

		public ObservableCollection<Comment> CommentsItems
		{
			get { return commentsItems; }
			set { commentsItems = value; OnPropertyChanged("CommentsItems"); }
		}

		private string news_id;

		private string author_name = string.Empty;

		public string Author_name
		{
			get { return author_name; }
			set { SetProperty (ref author_name, value, "Author_name");}
		}

		public DateTime PublishTime { get; set; }

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

		private string yourcomment = string.Empty;

		public string YourComment
		{
			get { return yourcomment; }
			set { SetProperty (ref yourcomment, value, "YourComment");}
		}

		private string error = string.Empty;

		public string Error
		{
			get { return error; }
			set { SetProperty (ref error, value, "Error");}
		}

		private string comment_author_id { get; set; }

		private string comment_author_name { get; set; }

		private Command loadItemsCommand;
		/// <summary>
		/// Command to load/refresh items
		/// </summary>
		public Command LoadItemsCommand
		{
			get { return loadItemsCommand ?? (loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand())); }
		}

		public ICommand PostCommand { protected set; get; }

		public ICommand LikeCommand { protected set; get; }

		public NewsDetailsViewModel (string newsID, string authorName, DateTime publishTime, string text, 
									 string pic_url, string c_author_id, string c_author_name)
		{
			Title = "News Details";

			news_id = newsID;
			Author_name = authorName;
			PublishTime = publishTime;
			Text = text;
			Pic_url = pic_url;
			comment_author_id = c_author_id;
			comment_author_name = c_author_name;

			//Replace icon
			//Icon = "blog.png";

			this.PostCommand = new Command (async (nothing) => {
				try {
					Comment aComment = new Comment();

					aComment.Author_id = comment_author_id;
					aComment.Author_name = comment_author_name;
					aComment.NewsID = news_id;
					aComment.Text = yourcomment;
					aComment.PublishTime = DateTime.Now;
					aComment.Liked = false;

					JToken payload = aComment.ToJToken();

					var resultJson = await App.ServiceClient.InvokeApiAsync("table/comment", payload);

					string rowkey = resultJson.Value<string>("rowkey");

					aComment.Id = rowkey;

					CommentsItems.Add(aComment);

					Error = "Added";
				} catch (Exception ex)
				{
					string str = ex.Message;
				}
			});
		}

		public async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try{
				CommentsItems.Clear();

				var resultComments = await App.ServiceClient.InvokeApiAsync("table/comment/" + news_id, HttpMethod.Get, null);

				if (resultComments.HasValues)
				{
					foreach (var item in resultComments)
					{
						if (item is JObject) {
							CommentsItems.Add(new Comment(item as JObject));
						} else {
							throw new Exception("Unexpected type in resultComments");
						}
					}
				}
				else 
				{
					throw new Exception("Nothing returned!");
				}
			} catch (Exception ex) {
				string str = ex.Message;
			}

			IsBusy = false;
		}
	}
}
