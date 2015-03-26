using System;
using System.Collections;
using System.Collections.Generic;

namespace App
{
	public class AddHorseItem : MenuItem
	{
		public override string Title { get { return "Add Horse"; } }
	}

	public class NewsFeedItem : MenuItem
	{
		public override string Title { get { return "News Feed"; } }
	}

	public class LogOutItem : MenuItem
	{
		public override string Title { get { return "Log Out"; } }
	}

	public class MessagesItem : MenuItem
	{
		public override string Title { get { return "Messages"; } }
	}

	public class NotificationsItem : MenuItem
	{
		public override string Title { get { return "Notifications"; } }
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

			aList.Add (new AddHorseItem ());
			aList.Add (new NewsFeedItem ());
			aList.Add (new MessagesItem ());
			aList.Add (new NotificationsItem ());
			aList.Add (new LogOutItem ());

			All = aList;
		}

		public static IEnumerable<MenuItem> All { private set; get; }
	}
}
