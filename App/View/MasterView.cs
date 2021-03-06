﻿using System;
using Xamarin.Forms;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	/// <summary>
	/// View of the main MasterDetail page.
	/// </summary>
	public class MasterView : MasterDetailPage
	{
		private MasterViewModel ViewModel
		{
			get { return BindingContext as MasterViewModel; }
		}

		MenuView master;
		private Dictionary<MenuType, NavigationPage> pages;

		public MasterView (MeVM me)
		{
			pages = new Dictionary<MenuType, NavigationPage>();
			BindingContext = new MasterViewModel(me);

			Master = master = new MenuView(ViewModel);

			var homeNav = new NavigationPage(master.PageSelection)
			{
				BarBackgroundColor = Color.FromHex("#3498DB"),
				BarTextColor = Color.White
			};
			Detail = homeNav;

			pages.Add(MenuType.NewsFeed, homeNav);

			master.PageSelectionChanged = (menuType) =>
			{
				NavigationPage newPage;
				if (pages.ContainsKey(menuType))
				{
					newPage = pages[menuType];
				}
				else
				{
					newPage = new NavigationPage(master.PageSelection)
					{
						BarBackgroundColor = Color.FromHex("#3498DB"),
						BarTextColor = Color.White
					};
					pages.Add(menuType, newPage);
				}
				Detail = newPage;
				Detail.Title = master.PageSelection.Title;
				IsPresented = false;
			};

			this.Icon = "slideout.png";
		}
	}

	/// <summary>
	/// View of the MasterDetail page's menu.
	/// </summary>
	public class MenuView : BaseView
	{
		public Action<MenuType> PageSelectionChanged;
		private Page pageSelection;
		private MenuType menuType = MenuType.Stable;
		public Page PageSelection
		{
			get { return pageSelection; }
			set
			{
				pageSelection = value;
				if (PageSelectionChanged != null)
					PageSelectionChanged(menuType);
			}
		}

		private Page about;

		public MenuView(MasterViewModel viewModel)
		{
			this.Icon = "slideout.png";
			BindingContext = viewModel;

			var layout = new StackLayout { Spacing = 0, BackgroundColor = Color.White};

			var label = new ContentView
			{
				Padding = new Thickness(10, 36, 0, 5),
				BackgroundColor = Color.Transparent,
				Content = new Label
				{
					Text = "HorseFriends",
					FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
					TextColor = Color.Black
				}
			};

			layout.Children.Add(label);

			var profile_pic = new Image ();

			profile_pic.Source = viewModel.pic_url;
			profile_pic.HorizontalOptions = LayoutOptions.FillAndExpand;

			layout.Children.Add (profile_pic);

			var name = new Label ();

			name.Text = viewModel.name;
			name.HorizontalOptions = LayoutOptions.Center;
			name.TextColor = Color.Black;

			layout.Children.Add (name);

			var listView = new ListView();

			DataTemplate cell = null;

			cell = new DataTemplate(typeof(ImageCell));
			cell.SetBinding(TextCell.TextProperty, MasterViewModel.TitlePropertyName);
			cell.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			cell.SetValue (TextCell.TextColorProperty, Color.Black);

			listView.ItemTemplate = cell;

			listView.ItemsSource = viewModel.MenuItems;
			if (about == null)
				about = new NewsView(viewModel.id, viewModel.horse_id, viewModel.name, viewModel.horse_name, viewModel.pic_url);

			PageSelection = about;
			//Menu selection interactions.
			listView.ItemSelected += (sender, args) =>
			{
				var menuItem = listView.SelectedItem as MenuItem;
				menuType = menuItem.MenuType;
				switch (menuItem.MenuType)
				{
					case MenuType.NewsFeed:
						PageSelection = new NewsView(viewModel.id, viewModel.horse_id, viewModel.name, viewModel.horse_name, viewModel.pic_url);
						break;
					case MenuType.Stable:
						PageSelection = new HorseView(viewModel.id, viewModel.horse_id);
						break;
					case MenuType.Notification:
						PageSelection = new NotificationsView(viewModel.id);
						break;
					case MenuType.LogOut:
						DependencyService.Get<IMobileClient> ().ResetCachedToken (MobileServiceAuthenticationProvider.Facebook);
						DependencyService.Get<IMobileClient>().Logout();
						
						Navigation.PopModalAsync();
						break;
					default:
						break;
				}
			};

			listView.SelectedItem = viewModel.MenuItems[1];
			layout.Children.Add(listView);

			Content = layout;
		}
	}
}
