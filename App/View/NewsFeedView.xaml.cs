using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class NewsFeedView : ContentPage
	{
		private NewsFeedViewModel ViewModel
		{
			get { return BindingContext as NewsFeedViewModel; }
		}

		public NewsFeedView ()
		{
			InitializeComponent ();

			BindingContext = new NewsFeedViewModel();

			listView.ItemTapped += (sender, args) =>
			{
				if (listView.SelectedItem == null)
					return;
				//Here goes to the detail page
				//this.Navigation.PushAsync(new BlogDetailsView(listView.SelectedItem as FeedItem));
				listView.SelectedItem = null;
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy || ViewModel.FeedItems.Count > 0)
				return;

			ViewModel.LoadItemsCommand.Execute(null);
		}
	}
}
