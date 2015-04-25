using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace App
{
	public class AddNewsViewModel : BaseViewModel
	{
		private string AuthorID { get; set;}
		private string HorseID { get; set; }
		private News news { get; set; }
		private byte[] imageBytes { get; set; }

		public AddNewsViewModel (string author_id, string horse_id)
		{
			Title = "Stable";
			Icon = "blog.png";
			AuthorID = author_id;
			HorseID = horse_id;

			this.PostCommand = new Command (async (nothing) => {
				//Post something.
			});
		}

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

		public ICommand PostCommand { protected set; get; }

		public async Task LoadValues()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			//Load the values

			IsBusy = false;
		}
	}
}
