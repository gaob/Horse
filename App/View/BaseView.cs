using System;
using Xamarin.Forms;

namespace App
{
	/// <summary>
	/// Base class of all views.
	/// </summary>
	public class BaseView : ContentPage
	{
		public BaseView ()
		{
			SetBinding (Page.TitleProperty, new Binding(BaseViewModel.TitlePropertyName));
			SetBinding (Page.IconProperty, new Binding(BaseViewModel.IconPropertyName));
		}
	}
}
