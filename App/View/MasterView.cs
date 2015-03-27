using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace App
{
	public class MasterView : MasterDetailPage
	{
		private MasterViewModel ViewModel
		{
			get { return BindingContext as MasterViewModel; }
		}

		MenuView master;
		private Dictionary<MenuType, NavigationPage> pages;

		public MasterView ()
		{
			pages = new Dictionary<MenuType, NavigationPage>();
			BindingContext = new MasterViewModel();

			Master = master = new MenuView(ViewModel);

			var homeNav = new NavigationPage(master.PageSelection)
			{
				BarBackgroundColor = Color.FromHex("#3498DB"),
				BarTextColor = Color.White
			};
			Detail = homeNav;

			pages.Add(MenuType.About, homeNav);

			master.PageSelectionChanged = async (menuType) =>
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
				if(Device.Idiom != TargetIdiom.Tablet)
					IsPresented = false;
			};

			this.Icon = "slideout.png";
		}
	}

	public class MenuView : BaseView
	{
		public Action<MenuType> PageSelectionChanged;
		private Page pageSelection;
		private MenuType menuType = MenuType.About;
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

		private Page about, blog, twitter, hanselminutes, ratchet, developerlife;

		public MenuView(MasterViewModel viewModel)
		{
			this.Icon = "slideout.png";
			BindingContext = viewModel;


			var layout = new StackLayout { Spacing = 0 };

			var label = new ContentView
			{
				Padding = new Thickness(10, 36, 0, 5),
				BackgroundColor = Color.Transparent,
				Content = new Label
				{
					Text = "MENU",
					FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
				}
			};

			layout.Children.Add(label);

			var listView = new ListView();

			DataTemplate cell = null;

			cell = new DataTemplate(typeof(ImageCell));
			cell.SetBinding(TextCell.TextProperty, MasterViewModel.TitlePropertyName);
			cell.SetBinding(ImageCell.ImageSourceProperty, "Icon");

			listView.ItemTemplate = cell;

			listView.ItemsSource = viewModel.MenuItems;
			if (about == null)
				about = new UnevenRowsXaml();

			PageSelection = about;
			//Change to the correct page
			listView.ItemSelected += (sender, args) =>
			{
				PageSelection = about;
			};

			listView.SelectedItem = viewModel.MenuItems[0];
			layout.Children.Add(listView);

			Content = layout;
		}
	}
}
