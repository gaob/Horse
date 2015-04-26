using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace App
{
	public class NewsViewModel : BaseViewModel
	{
		private ObservableCollection<News> newsItems = new ObservableCollection<News>();

		public ObservableCollection<News> NewsItems
		{
			get { return newsItems; }
			set { newsItems = value; OnPropertyChanged("NewsItems"); }
		}

		private Command loadItemsCommand;
		/// <summary>
		/// Command to load/refresh items
		/// </summary>
		public Command LoadItemsCommand
		{
			get { return loadItemsCommand ?? (loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand())); }
		}

		public NewsViewModel ()
		{
			Title = "News Feed";
			//Replace icon
			//Icon = "blog.png";
		}

		public async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try{
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
			} catch (Exception ex) {
				string str = ex.Message;
			}

			IsBusy = false;
		}
	}
}
