﻿using System;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public class MobileServiceHelper
	{
		private static MobileServiceHelper _instance;

		const string applicationURL = @"https://dotnet3.azure-mobile.net/";
		const string applicationKey = @"KjlDtnWMUnoQiFuxmlxdhYIMEUOnDs87";

		private readonly MobileServiceClient _client;

		private MobileServiceHelper()
		{
			// Initialize the Mobile Service client with your URL and key
			_client = new MobileServiceClient(applicationURL, applicationKey);

		}

		private static volatile object _syncRoot = new object();

		public MobileServiceClient ServiceClient { get { return _client; } }

		public static MobileServiceHelper DefaultService
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncRoot)
					{
						if (_instance == null)
						{
							_instance = new MobileServiceHelper();
						}
					}
				}

				return _instance;
			}
		}

	}
}
