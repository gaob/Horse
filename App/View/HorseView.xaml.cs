﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public partial class HorseView : ContentPage
	{
		private IMobileServiceTable<HorseItem> HorseTable;

		public HorseView ()
		{
			InitializeComponent ();
		}

		async void OnGetClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				valueLabel.Text = "Wrong!";
			}
			else
			{
				try
				{
					HorseTable = App.ServiceClient.GetTable<HorseItem> ();

					HorseItem aHorse = new HorseItem (null);

					await HorseTable.InsertAsync (aHorse);

					valueLabel.Text = "Nothing returned!";
				}
				catch (Exception ex)
				{
					// Display the exception message for the demo
					valueLabel.Text = ex.Message;
				}
			}
		}
	}
}
