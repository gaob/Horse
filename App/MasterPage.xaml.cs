using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class MasterPage : MasterDetailPage
	{
		public MasterPage (MeVM me)
		{
			InitializeComponent ();

			NavigationPage.SetHasNavigationBar (this, false);

			BindingContext = new {
				Menu = new { Subtitle = me.name, 
					Url = me.pic_url},
				Detailpage = new { Subtitle = "I'm Detail" }
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing ();
		}

		async void OnGetClicked(object sender, EventArgs args)
		{
			IsPresented = true;
		}
	}
}
