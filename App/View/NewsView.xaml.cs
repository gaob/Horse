﻿using System;
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

		public NewsView (string author_id, string theID, string author_name, string author_pic_url)
		{
			InitializeComponent ();

			ToolbarItems.Add (new ToolbarItem ("Filter", "blog.png", async () => {
				await this.Navigation.PushAsync(new AddNewsView(author_id, theID, author_name, author_pic_url));
			}));

			BindingContext = new NewsViewModel ();

			listView.ItemTapped += (sender, args) =>
			{
				if (listView.SelectedItem == null)
					return;
				this.Navigation.PushAsync(new NewsDetailsView(listView.SelectedItem as News));
				listView.SelectedItem = null;
			};
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
