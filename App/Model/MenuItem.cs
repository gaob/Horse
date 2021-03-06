﻿using System;
using Xamarin.Forms;

namespace App
{
	public enum MenuType
	{
		Stable,
		NewsFeed,
		Notification,
		LogOut
	}

	/// <summary>
	/// Menu item class
	/// </summary>
	public class MenuItem
	{
		public MenuItem ()
		{
			MenuType = MenuType.Stable;
		}

		//Base fields
		public string Title {get;set;}
		public string Details { get; set; }
		public int Id { get; set; }

		public string Icon {get;set;}
		public MenuType MenuType { get; set; }
	}
}
