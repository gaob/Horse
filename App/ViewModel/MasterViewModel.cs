using System;
using System.Collections.ObjectModel;

namespace App
{
	public class MasterViewModel : BaseViewModel
	{
		public ObservableCollection<HomeMenuItem> MenuItems { get; set; }

		//MeVM
		private MeVM itsMe { get; set; }
		public string pic_url { get { return itsMe.pic_url; } }
		public string name {get { return itsMe.name; } }

		public MasterViewModel(MeVM me)
		{
			CanLoadMore = true;
			Title = "HorseFriendsTitle";
			MenuItems = new ObservableCollection<HomeMenuItem>();
			MenuItems.Add(new HomeMenuItem
				{
					Id = 0,
					Title = "Add Horse",
					MenuType = MenuType.AddHorse,
					Icon = "about.png"
				});
			MenuItems.Add(new HomeMenuItem
				{
					Id = 1,
					Title = "News Feed",
					MenuType = MenuType.NewsFeed,
					Icon = "blog.png"
				});
			MenuItems.Add(new HomeMenuItem
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
