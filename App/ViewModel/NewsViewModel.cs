using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;

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
				News aNews = new News();

				aNews.Author_id = "01";
				aNews.Author_name = "Dancer Stark";
				aNews.Author_pic_url = "";
				aNews.Pic_url = "http://thumbs.dreamstime.com/z/rearing-horse-26766173.jpg";
				aNews.Text = "This is a horse!";
				aNews.PublishTime = DateTime.Now;

				NewsItems.Add(aNews);
			} catch (Exception ex) {
				string str = ex.Message;
			}

			IsBusy = false;
		}
	}
}
