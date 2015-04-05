using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace App
{
	public class NewsFeedViewModel : BaseViewModel
	{
		public NewsFeedViewModel ()
		{
			Title = "Blog";
			Icon = "blog.png";
		}

		private ObservableCollection<NewsItem> feedItems = new ObservableCollection<NewsItem>();

		/// <summary>
		/// gets or sets the feed items
		/// </summary>
		public ObservableCollection<NewsItem> FeedItems
		{
			get { return feedItems; }
			set { feedItems = value; OnPropertyChanged("FeedItems"); }
		}

		private NewsItem selectedFeedItem;
		/// <summary>
		/// Gets or sets the selected feed item
		/// </summary>
		public NewsItem SelectedFeedItem
		{
			get{ return selectedFeedItem; }
			set
			{
				selectedFeedItem = value;
				OnPropertyChanged("SelectedFeedItem");
			}
		}

		private Command loadItemsCommand;
		/// <summary>
		/// Command to load/refresh items
		/// </summary>
		public Command LoadItemsCommand
		{
			get { return loadItemsCommand ?? (loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand())); }
		}

		private async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try{
				var responseString = string.Empty;
				using(var httpClient = new HttpClient())
				{
					var feed = "http://feeds.hanselman.com/ScottHanselman";
					responseString = await httpClient.GetStringAsync(feed);
				}

				FeedItems.Clear();
				var items = await ParseFeed(responseString);
				foreach (var item in items)
				{
					FeedItems.Add(item);
				}
			} catch (Exception) {
				var page = new ContentPage();
				/*var result = */await page.DisplayAlert ("Error", "Unable to load blog.", "OK");
			}

			IsBusy = false;
		}

		/// <summary>
		/// Parse the RSS Feed
		/// </summary>
		/// <param name="rss"></param>
		/// <returns></returns>
		private async Task<List<NewsItem>> ParseFeed(string rss)
		{
			return await Task.Run(() =>
				{
					var xdoc = XDocument.Parse(rss);
					var id = 0;
					return (from item in xdoc.Descendants("item")
						select new NewsItem
						{
							Title = (string)item.Element("title"),
							Description = (string)item.Element("description"),
							Link = (string)item.Element("link"),
							PublishDate = (string)item.Element("pubDate"),
							Category = (string)item.Element("category"),
							Id = id++
						}).ToList();
				});
		}

		/// <summary>
		/// Gets a specific feed item for an Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public NewsItem GetFeedItem(int id)
		{
			return FeedItems.FirstOrDefault(i => i.Id == id);
		}
	}
}
