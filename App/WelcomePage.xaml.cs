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

		async Task InitializeLoginButton ()
		{
			LoginB.Text = "Loading your info";

			await DependencyService.Get<IMobileClient> ().RetrieveCachedToken (MobileServiceAuthenticationProvider.Facebook);

			if (App.ServiceClient.CurrentUser == null || App.ServiceClient.CurrentUser.UserId == null) {
				LoginB.Text = "Log In Via Facebook";
			} else {
				//valueLabel.Text = string.Format("You are now logged in - {0}", App.ServiceClient.CurrentUser.UserId);

				LoginB.Text = "Welcome Back!";

				await ContinueToMain ();

				LoginB.Text = "Log In Via Facebook";
			}
		}

		async void OnLoginClicked(object sender, EventArgs args)
		{
			await AuthenticateUserCachedTokenAsync();

			await ContinueToMain ();
		}

		async Task ContinueToMain()
		{
			/*
			await Unit_Test ();
			return;
			*/

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

						await Navigation.PushModalAsync(new MasterView(me));
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
		}
			
		async Task Unit_Test ()
		{
			try
			{
				//Unit Test Facebook user email: jdeqyhg_changsky_1426475025 @ tfbnw.net
				//Password: 1

				// Use test account to test get me.
				var resultJson = await App.ServiceClient.InvokeApiAsync("me", HttpMethod.Get, null);

				Assert_True(resultJson.HasValues, "me GET: nothing returned!");

				MeVM me = new MeVM(resultJson as JObject);

				Assert_True(me.Id == "1379198605736516", "me Id error!");
				Assert_True(me.Name == "Lisa Amiciafhdggj Changsky", "me Name error!");

				// test add horse.
				var horse = new HorseItem();
				horse.Name = "Test Horse";
				horse.Owner_id = "1379198605736516";
				horse.Gender = "Test Gender";
				horse.Year = 2015;
				horse.Breed = "Test Breed";
				horse.Registered = "Test Registered";
				horse.Description = "Test Description";

				IMobileServiceTable<HorseItem> HorseTable = App.ServiceClient.GetTable<HorseItem> ();

				await HorseTable.InsertAsync(horse);

				string horse_id = horse.Id;

				resultJson = await App.ServiceClient.InvokeApiAsync("me", HttpMethod.Get, null);

				Assert_True(resultJson.HasValues, "me GET: nothing returned!");

				me = new MeVM(resultJson as JObject);

				Assert_True(MeVM.Horse_id == horse_id, "Add horse: id error!");
				Assert_True(MeVM.Horse_name == horse.Name, "Add horse: name error!");

				// test update horse.
				horse.Name = "Test Horse 2";
				await HorseTable.UpdateAsync(horse);

				resultJson = await App.ServiceClient.InvokeApiAsync("me", HttpMethod.Get, null);

				Assert_True(resultJson.HasValues, "me GET: nothing returned!");

				me = new MeVM(resultJson as JObject);

				Assert_True(MeVM.Horse_name == "Test Horse 2", "Update horse: name error!");

				await HorseTable.DeleteAsync(horse);

				valueLabel.Text = "Unit Tests passed";
			}
			catch (Exception ex)
			{
				// Display the exception message for unit testing.
				valueLabel.Text = ex.Message;
			}
		}

		void Assert_True(bool value, string message)
		{
			if (!value) {
				throw new Exception (message);
			}
		}
	}
}
