using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public interface IMobileClient
	{
		Task<MobileServiceUser> Authorize(MobileServiceAuthenticationProvider provider);
		void Logout();
	}
}

