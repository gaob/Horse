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

			Master.BackgroundColor = Color.FromHex("333333");

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
	}
}
