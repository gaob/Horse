using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class NewsView : ContentPage
	{
		private NewsViewModel ViewModel
		{
			get { return BindingContext as NewsViewModel; }
		}

		public NewsView ()
		{
			InitializeComponent ();

			BindingContext = new NewsViewModel ();

			/*
			listView.ItemTapped += (sender, args) =>
			{
				if (listView.SelectedItem == null)
					return;
				//Here goes to the detail page
				//this.Navigation.PushAsync(new BlogDetailsView(listView.SelectedItem as FeedItem));
				listView.SelectedItem = null;
			};
			*/
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy)
				return;

			await ViewModel.ExecuteLoadItemsCommand ();
		}
	}
}
