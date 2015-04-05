using System;
using System.Collections.ObjectModel;

namespace App
{
	public class MasterViewModel : BaseViewModel
	{
		public ObservableCollection<MenuItem> MenuItems { get; set; }

		//MeVM
		private MeVM itsMe { get; set; }
		public string pic_url { get { return itsMe.Pic_url; } }
		public string name {get { return itsMe.Name; } }

		public MasterViewModel(MeVM me)
		{
			CanLoadMore = true;
			Title = "HorseFriendsTitle";
			MenuItems = new ObservableCollection<MenuItem>();
			MenuItems.Add(new MenuItem
				{
					Id = 0,
					Title = "Add Horse",
					MenuType = MenuType.AddHorse,
					Icon = "about.png"
				});
			MenuItems.Add(new MenuItem
				{
					Id = 1,
					Title = "News Feed",
					MenuType = MenuType.NewsFeed,
					Icon = "blog.png"
				});
			MenuItems.Add(new MenuItem
				{
					Id = 2,
					Title = "Log Out",
					MenuType = MenuType.LogOut,
					Icon = "twitternav.png"
				});

			itsMe = me;
		}
	}
}
