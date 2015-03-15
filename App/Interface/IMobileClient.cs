using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public interface IMobileClient
	{
		Task<MobileServiceUser> Authorize(MobileServiceAuthenticationProvider provider);
		void Logout();
		Task RetrieveCachedToken(MobileServiceAuthenticationProvider provider);
		bool SaveCachedToken(MobileServiceUser theUser, MobileServiceAuthenticationProvider provider);
		void ResetCachedToken (MobileServiceAuthenticationProvider provider);
	}
}
