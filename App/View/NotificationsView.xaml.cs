using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class NotificationsView : ContentPage
	{
		private NotificationsViewModel ViewModel
		{
			get { return BindingContext as NotificationsViewModel; }
		}

		public NotificationsView (string user_id)
		{
			InitializeComponent ();

			BindingContext = new NotificationsViewModel (user_id);
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
