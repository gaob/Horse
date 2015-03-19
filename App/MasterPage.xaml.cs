using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace App
{
	public partial class MasterPage : MasterDetailPage
	{
		public MasterPage (string url)
		{
			InitializeComponent ();

			BindingContext = new {
				Menu = new { Subtitle = "I'm Master", 
					Url = url},
				Detailpage = new { Subtitle = "I'm Detail" }
			};
		}
	}
}

