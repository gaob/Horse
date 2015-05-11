using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App
{
	/// <summary>
	/// View Model to add news.
	/// </summary>
	public class AddNewsViewModel : BaseViewModel
	{
		private string AuthorID { get; set;}
		private string HorseID { get; set; }
		private News news { get; set; }
		private byte[] imageBytes { get; set; }
		private INavigation Navigation { get; set; }

		public AddNewsViewModel (string author_id, string horse_id, string author_name, string horse_name, string author_pic_url, INavigation navi)
		{
			Title = "Stable";
			Icon = "blog.png";
			AuthorID = author_id;
			HorseID = horse_id;
			Navigation = navi;

			this.PostCommand = new Command (async (nothing) => {
				try {
					//To prevent add when no image present.
					News aNews = new News();
					aNews.Author_id = AuthorID;
					aNews.Author_name = author_name;
					aNews.Author_pic_url = author_pic_url;
					aNews.Text = Text;
					aNews.Pic_url = string.Empty;
					aNews.PublishTime = DateTime.Now;
					aNews.Horse_id = MeVM.Horse_id;
					aNews.Horse_name = MeVM.Horse_name;

					JToken payload = aNews.ToJToken();

					var resultJson = await App.ServiceClient.InvokeApiAsync("table/news", payload);

					string rowkey = resultJson.Value<string>("rowkey");

					if (imageBytes != null) {
						#pragma warning disable 4014
						// Uploading image doesn't need to be awaited in order to reduce client response time.
						RemoteBlobAccess.uploadToBlobStorage_async(imageBytes, "news-" + rowkey + ".jpg");
						#pragma warning restore 4014
					}

					Error = "Added";

					await Navigation.PopAsync();
				} catch (Exception ex)
				{
					string str = ex.Message;
				}
			});
		}

		/// <summary>
		/// Method to receive image bytes from View.
		/// </summary>
		/// <param name="theImageBytes">The image bytes.</param>
		public void passImageBytes(byte[] theImageBytes)
		{
			imageBytes = theImageBytes;
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

		private string error = string.Empty;

		public string Error
		{
			get { return error; }
			set { SetProperty (ref error, value, "Error");}
		}

		public ICommand PostCommand { protected set; get; }
	}
}
