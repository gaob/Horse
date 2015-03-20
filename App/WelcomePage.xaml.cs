using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace App
{
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();

			NavigationPage.SetHasNavigationBar (this, false);
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing ();

			await InitializeLoginButton ();
		}

		//May need to put it in OnAppearing()
		async Task InitializeLoginButton ()
		{
			GetB.Text = "Get";

			await DependencyService.Get<IMobileClient> ().RetrieveCachedToken (MobileServiceAuthenticationProvider.Facebook);

			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null) {
				LoginB.Text = "Log in";
			} else {
				valueLabel.Text = string.Format("You are now logged in - {0}", App.ServiceClient.CurrentUser.UserId);

				LoginB.Text = "Log out";
			}
		}
		
		async void OnLoginClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				// DEMO 2 - Cached credentials using the key chain
				await AuthenticateUserCachedTokenAsync();
			}
			else
			{
				DependencyService.Get<IMobileClient> ().ResetCachedToken (MobileServiceAuthenticationProvider.Facebook);
				DependencyService.Get<IMobileClient>().Logout();

				valueLabel.Text = "Logged out";
			}
		}

		async void OnGetClicked(object sender, EventArgs args)
		{
			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null)
			{
				valueLabel.Text = "Wrong!";
			}
			else
			{
				try
				{
					// Create the json to send using an anonymous type 
					//JToken payload = JObject.FromObject(new { msg = "facebook" });

					// Make the call to the hello resource asynchronously using POST verb
					var resultJson = await App.ServiceClient.InvokeApiAsync("me", HttpMethod.Get, null);

					// Verfiy that a result was returned
					if (resultJson.HasValues)
					{
						MeVM me = new MeVM(resultJson as JObject);

						valueLabel.Text = "Gotten";

						await Navigation.PushAsync(new MasterPage(me));
					}
					else
					{
						valueLabel.Text = "Nothing returned!";
					}
				}
				catch (Exception ex)
				{
					// Display the exception message for the demo
					valueLabel.Text = ex.Message;
				}
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
