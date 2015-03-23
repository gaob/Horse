using System;
using System.Collections;
using System.Collections.Generic;

namespace App
{
	public class NewsFeedItem : MenuItem
	{
		public override string Title { get { return "News Feed"; } }
	}

	public class LogOutItem : MenuItem
	{
		public override string Title { get { return "Log Out"; } }
	}

	public abstract class MenuItem
	{
		public virtual string Title { get; set; }
		public virtual bool Selected { get; set; }
	}

	public class MenuItems
	{
		static MenuItems()
		{
			List<MenuItem> aList = new List<MenuItem> ();

			aList.Add (new NewsFeedItem ());
			aList.Add (new LogOutItem ());

			All = aList;
		}

		public static IEnumerable<MenuItem> All { private set; get; }
	}
}
