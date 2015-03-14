using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Foundation;
using UIKit;
using App.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(iOSMobileClient))]
namespace App.iOS
{
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
	}
}
