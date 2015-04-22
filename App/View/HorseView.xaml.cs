using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Media.Plugin;
using System.IO;

namespace App
{
	public partial class HorseView : ContentPage
	{
		private HorseViewModel ViewModel
		{
			get { return BindingContext as HorseViewModel; }
		}

		public HorseView (string owner_id, string theID)
		{
			InitializeComponent ();

			BindingContext = new HorseViewModel (owner_id, theID);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy)
				return;

			await ViewModel.LoadValues ();
		}

		async void OnTakePhotoClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				valueLabel.Text = "Wrong!";
			}
			else
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
							Directory = "Sample",
							Name = "test.jpg"
						});

					if (file == null)
						return;

					var stream = file.GetStream();

					byte[] imageBytes = ReadFully(stream);

					image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

					file.Dispose();
				}
				catch (Exception ex)
				{
					// Display the exception message for the demo
					valueLabel.Text = ex.Message;
				}
			}
		}

		async void OnPickPhotoClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				valueLabel.Text = "Wrong!";
			}
			else
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

					byte[] imageBytes = ReadFully(stream);

					image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

					file.Dispose();

					/*
					HorseTable = App.ServiceClient.GetTable<HorseItem> ();

					HorseItem aHorse = new HorseItem (null);

					await HorseTable.InsertAsync (aHorse);

					valueLabel.Text = "Nothing returned!";
					*/
				}
				catch (Exception ex)
				{
					// Display the exception message for the demo
					valueLabel.Text = ex.Message;
				}
			}
		}

		//Check another copy method
		public static byte[] ReadFully(Stream input) {
			byte[] buffer = new byte[16*1024];
			using(MemoryStream ms = new MemoryStream()) {
				int read;
				while((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}
