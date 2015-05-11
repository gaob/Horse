using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	/// <summary>
	/// mobile client interfaces in the shared code, detailed implementations are in each client code.
	/// </summary>
	public interface IMobileClient
	{
		Task<MobileServiceUser> Authorize(MobileServiceAuthenticationProvider provider);
		void Logout();
		Task RetrieveCachedToken(MobileServiceAuthenticationProvider provider);
		bool SaveCachedToken(MobileServiceUser theUser, MobileServiceAuthenticationProvider provider);
		void ResetCachedToken (MobileServiceAuthenticationProvider provider);
	}
}
