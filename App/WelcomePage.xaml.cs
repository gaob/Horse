using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
		}

		async void OnButtonClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				// DEMO 1 - Non cached authentication
				await AuthenticateUserAsync();

				// DEMO 2 - Cached credentials using the key chain
				//await AuthenticateUserCachedTokenAsync();
			}
			else
			{
				DependencyService.Get<IMobileClient>().Logout();

				valueLabel.Text = "Logged out";
			}
		}

		/// <summary>
		/// authenticate user as an asynchronous operation.
		/// </summary>
		/// <param name="providerType">Type of the provider.</param>
		/// <returns>Task.</returns>
		public async Task AuthenticateUserAsync(MobileServiceAuthenticationProvider providerType = MobileServiceAuthenticationProvider.Facebook)
		{
			while (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				string message;

				try
				{
					// Authenticate using provider type passed in. 
					await DependencyService.Get<IMobileClient>().Authorize(providerType);
					message = string.Format("You are now logged in - {0}", App.ServiceClient.CurrentUser.UserId);
				}
				catch (InvalidOperationException ex)
				{
					message = "You must log in. Login Required" + ex.Message;
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}

				valueLabel.Text = message;
			}
		}
	}
}

