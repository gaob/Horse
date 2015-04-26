using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class NewsDetailsView : ContentPage
	{
		private NewsDetailsViewModel ViewModel
		{
			get { return BindingContext as NewsDetailsViewModel; }
		}

		public NewsDetailsView (News theNews, string author_id, string author_name)
		{
			InitializeComponent ();

			BindingContext = new NewsDetailsViewModel (theNews.Id, 
													   theNews.Author_name, 
													   theNews.PublishTime,
													   theNews.Text,
													   theNews.Pic_url,
													   author_id,
													   author_name);
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
