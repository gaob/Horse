using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Foundation;
using UIKit;
using App.iOS;
using Security;
using System.Net.Http;

[assembly: Xamarin.Forms.Dependency(typeof(iOSMobileClient))]
namespace App.iOS
{
	/// <summary>
	/// Client dependent implementations of IMobileClient interfaces.
	/// </summary>
	public class iOSMobileClient : IMobileClient
	{
		public async Task<MobileServiceUser> Authorize(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider provider)
		{
			return await App.ServiceClient.LoginAsync (UIApplication.SharedApplication.KeyWindow.RootViewController, provider);
		}

		public void Logout()
		{
			foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
				NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);

			App.ServiceClient.Logout ();
		}

		public async Task RetrieveCachedToken(MobileServiceAuthenticationProvider provider)
		{
			// Using the provider type as the lookup id here. The lookup id is scoped to the application
			// If the app were to support multiple users something like an email address or some other user 
			// identity could be used to support multiple key chain entrties for the same app and provider
			SecRecord credentialLookupRec = new SecRecord(SecKind.GenericPassword)
			{
				Generic = NSData.FromString(provider.ToString())
			};

			SecStatusCode statusCodeResult;
			var match = SecKeyChain.QueryAsRecord(credentialLookupRec, out statusCodeResult);

			// If there are credentials in the the key chain, verify that it's still valid
			if (statusCodeResult == SecStatusCode.Success)
			{
				App.ServiceClient.CurrentUser = await GetUserFromKeyChain(match.Account, match.ValueData.ToString(), credentialLookupRec);
			}

		}

		/// <summary>
		/// Gets the user from key chain.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <param name="authToken">The authentication token.</param>
		/// <param name="credentialLookupRec">The credential lookup record.</param>
		/// <returns>Task&lt;MobileServiceUser&gt;.</returns>
		private async Task<MobileServiceUser> GetUserFromKeyChain(string userId, string authToken, SecRecord credentialLookupRec)
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
					SecKeyChain.Remove(credentialLookupRec);
					mobileServiceUser = null;
				}
			}

			return mobileServiceUser;
		}

		public bool SaveCachedToken(MobileServiceUser theUser, MobileServiceAuthenticationProvider provider)
		{
			// Create the credential package to store in the preferences
			var credentialsRecord = new SecRecord(SecKind.GenericPassword)
			{
				Label = provider.ToString() + " Credentials",
				Description = "Cached mobile service credentials",
				Account = theUser.UserId,
				Service = "Mobile Service",
				Comment = "CSCI-E64",
				ValueData = NSData.FromString(theUser.MobileServiceAuthenticationToken),
				Generic = provider.ToString()
			};

			// Persist the entry in the key chain
			var result = SecKeyChain.Add(credentialsRecord);

			// Verify that it persisted 
			if (result != SecStatusCode.Success && result != SecStatusCode.DuplicateItem)
			{
				return false;
			}

			return true;
		}

		public void ResetCachedToken (MobileServiceAuthenticationProvider provider)
		{
			SecRecord credentialLookupRec = new SecRecord(SecKind.GenericPassword)
			{
				Generic = NSData.FromString(provider.ToString())
			};

			SecKeyChain.Remove(credentialLookupRec);
		}
	}
}
