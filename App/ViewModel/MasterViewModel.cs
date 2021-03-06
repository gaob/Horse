﻿using System;
using System.Collections.ObjectModel;

namespace App
{
	/// <summary>
	/// View Model for the MasterDetailPage View.
	/// </summary>
	public class MasterViewModel : BaseViewModel
	{
		public ObservableCollection<MenuItem> MenuItems { get; set; }

		//MeVM
		private MeVM itsMe { get; set; }
		public string id { get { return itsMe.Id; }}
		public string pic_url { get { return itsMe.Pic_url; } }
		public string name {get { return itsMe.Name; } }
		public string horse_id { get { return MeVM.Horse_id; } }
		public string horse_name { get { return MeVM.Horse_name; }}

		public MasterViewModel(MeVM me)
		{
			CanLoadMore = true;
			Title = "HorseFriendsTitle";
			MenuItems = new ObservableCollection<MenuItem>();
			MenuItems.Add(new MenuItem
				{
					Id = 0,
					Title = "Stable",
					MenuType = MenuType.Stable,
					Icon = "ic_play.png"
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
					Title = "Notifications",
					MenuType = MenuType.Notification,
					Icon = "about.png"
				});
			MenuItems.Add(new MenuItem
				{
					Id = 3,
					Title = "Log Out",
					MenuType = MenuType.LogOut,
					Icon = "refresh.png"
				});

			itsMe = me;
		}
	}
}
