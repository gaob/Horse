using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

[assembly: Xamarin.Forms.Dependency(typeof(App.Droid.AndroidMobileClient.AndroidLoginClient))]
namespace App.Droid
{
	public class AndroidMobileClient
	{
		public class AndroidLoginClient : IMobileClient
		{
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
		}
	}
}

