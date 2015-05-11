using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace App
{
	/// <summary>
	/// View Model of the Notifications View.
	/// </summary>
	public class NotificationsViewModel : BaseViewModel
	{
		private ObservableCollection<NotificationItem> notificationsItems = new ObservableCollection<NotificationItem>();

		public ObservableCollection<NotificationItem> NotificationsItems
		{
			get { return notificationsItems; }
			set { notificationsItems = value; OnPropertyChanged("NotificationsItems"); }
		}

		private string id = string.Empty;

		private Command loadItemsCommand;
		/// <summary>
		/// Command to load/refresh items
		/// </summary>
		public Command LoadItemsCommand
		{
			get { return loadItemsCommand ?? (loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand())); }
		}

		public NotificationsViewModel (string user_id)
		{
			Title = "Notifications Feed";
			Icon = "about.png";

			id = user_id;
		}

		/// <summary>
		/// Load Values of notification list from backend.
		/// </summary>
		/// <returns>The load items command.</returns>
		public async Task ExecuteLoadItemsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			try{
				NotificationsItems.Clear();

				var resultNews = await App.ServiceClient.InvokeApiAsync("table/notifications/" + id, HttpMethod.Get, null);

				if (resultNews.HasValues)
				{
					foreach (var item in resultNews)
					{
						if (item is JObject) {
							NotificationItem aNotification = new NotificationItem(item as JObject);

							NotificationsItems.Add(aNotification);
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
