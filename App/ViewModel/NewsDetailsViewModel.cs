using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;

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
									 string pic_url)
		{
			Title = "News Details";

			news_id = newsID;
			Author_name = authorName;
			PublishTime = publishTime;
			Text = text;
			Pic_url = pic_url;

			//Replace icon
			//Icon = "blog.png";
		}

		public async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try{
				/*
				NewsItems.Clear();

				var resultNews = await App.ServiceClient.InvokeApiAsync("table", HttpMethod.Get, null);

				if (resultNews.HasValues)
				{
					foreach (var item in resultNews)
					{
						if (item is JObject) {
							News anNews = new News(item as JObject);

							if (DateTime.Now>anNews.PublishTime.AddMinutes(1)) {
								NewsItems.Add(new News(item as JObject));
							}
						} else {
							throw new Exception("Unexpected type in resultNews");
						}
					}
				}
				else 
				{
					throw new Exception("Nothing returned!");
				}
				*/
			} catch (Exception ex) {
				string str = ex.Message;
			}

			IsBusy = false;
		}
	}
}
