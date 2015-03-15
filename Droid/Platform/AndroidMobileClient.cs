using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Android.Content;
using System.Net.Http;

[assembly: Xamarin.Forms.Dependency(typeof(App.Droid.AndroidMobileClient.AndroidLoginClient))]
namespace App.Droid
{
	public class AndroidMobileClient
	{
		public class AndroidLoginClient : IMobileClient
		{
			private static string USERID_PREFID = "userId";
			private static string AUTHTOKEN_PREFID = "authToken";

			public AndroidLoginClient() {}

			public async Task<MobileServiceUser> Authorize(MobileServiceAuthenticationProvider provider)
			{
				return await App.ServiceClient.LoginAsync(Xamarin.Forms.Forms.Context, provider);
			}

			public void Logout()
			{
				Android.Webkit.CookieSyncManager.CreateInstance(Android.App.Application.Context);
				Android.Webkit.CookieManager.Instance.RemoveAllCookie();

				App.ServiceClient.Logout ();
			}

			public async Task RetrieveCachedToken(MobileServiceAuthenticationProvider provider)
			{
				ISharedPreferences preferences = Xamarin.Forms.Forms.Context.GetSharedPreferences ("preferences", FileCreationMode.Private);

				// Try to get an existing credential from the preferences.

				string userId = preferences.GetString(USERID_PREFID, null);
				string authToken = preferences.GetString(AUTHTOKEN_PREFID, null);

				// There are credentials in the private preferences, get it from the preferences, verify that it's still valid
				if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(authToken))
				{ 
					App.ServiceClient.CurrentUser = await GetUserFromPreferences(userId, authToken, preferences);
				}
			}

			/// <summary>
			/// Gets the user from preferences.
			/// </summary>
			/// <param name="userId">The user identifier.</param>
			/// <param name="authToken">The authentication token.</param>
			/// <returns>Task&lt;MobileServiceUser&gt;.</returns>
			private async Task<MobileServiceUser> GetUserFromPreferences(string userId, string authToken, ISharedPreferences preferences)
			{
				// Create a user from the stored credentials.
				MobileServiceUser mobileServiceUser = new MobileServiceUser(userId);
				mobileServiceUser.MobileServiceAuthenticationToken = authToken;

				// Set the user from the stored credentials.
				App.ServiceClient.CurrentUser = mobileServiceUser;

				try
				{
					// Try to make a call to verify that the credential has not expired
					await App.ServiceClient.InvokeApiAsync("Hello", HttpMethod.Get, null);
				}
				catch (MobileServiceInvalidOperationException ex)
				{
					if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					{
						ISharedPreferencesEditor editor = preferences.Edit();

						// Remove the credentials with the expired token.
						editor.Remove(USERID_PREFID);
						editor.Remove(AUTHTOKEN_PREFID);
						editor.Commit();

						mobileServiceUser = null;
					}
				}

				return mobileServiceUser;
			}

			public bool SaveCachedToken(MobileServiceUser theUser, MobileServiceAuthenticationProvider provider)
			{
				ISharedPreferences preferences = Xamarin.Forms.Forms.Context.GetSharedPreferences ("preferences", FileCreationMode.Private);

				// Create the credential package to store in the preferences
				ISharedPreferencesEditor editor = preferences.Edit();
				editor.PutString(USERID_PREFID, theUser.UserId);
				editor.PutString(AUTHTOKEN_PREFID, theUser.MobileServiceAuthenticationToken);

				// Persist the credential package to the preferences
				editor.Commit();

				return true;
			}
		}
	}
}
