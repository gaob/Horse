using System;
using Xamarin.Forms;

namespace App
{
	public enum MenuType
	{
		AddHorse,
		NewsFeed,
		LogOut
	}

	public class MenuItem
	{
		public MenuItem ()
		{
			MenuType = MenuType.AddHorse;
		}

		//Base fields
		public string Title {get;set;}
		public string Details { get; set; }
		public int Id { get; set; }

		public string Icon {get;set;}
		public MenuType MenuType { get; set; }
	}
}
