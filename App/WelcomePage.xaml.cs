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

			InitializeLoginButton ();
		}

		async void InitializeLoginButton ()
		{
			await DependencyService.Get<IMobileClient> ().RetrieveCachedToken (MobileServiceAuthenticationProvider.Facebook);

			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null) {
				LoginB.Text = "Log in";
			} else {
				valueLabel.Text = string.Format("You are now logged in - {0}", App.ServiceClient.CurrentUser.UserId);

				LoginB.Text = "Log out";
			}
		}
		
		async void OnButtonClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				// DEMO 2 - Cached credentials using the key chain
				await AuthenticateUserCachedTokenAsync();
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
		public async Task AuthenticateUserCachedTokenAsync(MobileServiceAuthenticationProvider providerType = MobileServiceAuthenticationProvider.Facebook)
		{
			string message;

			while (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				try
				{
					// Authenticate using provider type passed in. 
					MobileServiceUser thisUser = await DependencyService.Get<IMobileClient>().Authorize(providerType);

					DependencyService.Get<IMobileClient>().SaveCachedToken(thisUser, providerType);
				}
				catch (InvalidOperationException ex)
				{
					message = "You must log in. Login Required" + ex.Message;
				}
				catch (Exception ex)
				{
					message = ex.Message;
				}
			}

			message = string.Format("You are now logged in - {0}", App.ServiceClient.CurrentUser.UserId);

			valueLabel.Text = message;
		}
	}
}
