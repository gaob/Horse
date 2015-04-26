using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Media.Plugin;
using System.IO;

namespace App
{
	public partial class AddNewsView : ContentPage
	{
		private AddNewsViewModel ViewModel
		{
			get { return BindingContext as AddNewsViewModel; }
		}

		public AddNewsView (string author_id, string horse_id, string author_name, string author_pic_url)
		{
			InitializeComponent ();

			BindingContext = new AddNewsViewModel (author_id, horse_id, author_name, author_pic_url, Navigation);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy)
				return;

			//Maybe it doesn't need to load values.
			await ViewModel.LoadValues ();
		}

		async void OnTakePhotoClicked(object sender, EventArgs args)
		{
			try
			{
				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
					{
						Directory = "temp",
						Name = "takenPhoto.jpg"
					});

				if (file == null)
					return;

				var stream = file.GetStream();

				byte[] imageBytes = PhotoHelper.ReadFully(stream);

				ViewModel.passImageBytes(imageBytes);

				image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

				file.Dispose();
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}

		async void OnPickPhotoClicked(object sender, EventArgs args)
		{
			try
			{
				if (!CrossMedia.Current.IsPickPhotoSupported)
				{
					await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
					return;
				}

				var file = await CrossMedia.Current.PickPhotoAsync();

				if (file == null)
					return;

				var stream = file.GetStream();

				byte[] imageBytes = PhotoHelper.ReadFully(stream);

				ViewModel.passImageBytes(imageBytes);

				image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

				file.Dispose();
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}
	}
}
