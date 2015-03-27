using System;

namespace App
{
	public class BaseModel
	{
		public BaseModel ()
		{
		}

		public string Title {get;set;}
		public string Details { get; set; }
		public int Id { get; set; }

	}

	public enum MenuType
	{
		About,
		Blog,
		Twitter,
		Hanselminutes,
		Ratchet,
		DeveloperLife
	}
	public class HomeMenuItem : BaseModel
	{
		public HomeMenuItem ()
		{
			MenuType = MenuType.About;
		}
		public string Icon {get;set;}
		public MenuType MenuType { get; set; }
	}
}

