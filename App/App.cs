using System;

using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public class App : Application
	{
		private static MobileServiceHelper client;

		public App ()
		{
			// The root page of your application
			MainPage = new WelcomePage ();

			client = MobileServiceHelper.DefaultService;
		}

		public static MobileServiceClient ServiceClient { get { return client.ServiceClient;}}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

