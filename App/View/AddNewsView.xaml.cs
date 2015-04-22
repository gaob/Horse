using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class AddNewsView : ContentPage
	{
		private AddNewsViewModel ViewModel
		{
			get { return BindingContext as AddNewsViewModel; }
		}

		public AddNewsView (string author_id, string horse_id)
		{
			InitializeComponent ();

			BindingContext = new AddNewsViewModel (author_id, horse_id);
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
		}

		async void OnPickPhotoClicked(object sender, EventArgs args)
		{
		}
	}
}
