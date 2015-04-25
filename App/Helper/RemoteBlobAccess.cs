﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
//using TML.BL.Managers;
//using TML.DL.SQLiteBase;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace App
{
	public class RemoteBlobAccess
	{
		public static class AzureStorageConstants
		{
			public static string Account = "dotnet3";
			public static string SharedKeyAuthorizationScheme = "SharedKey";
			public static string BlobEndPoint = "https://dotnet3.blob.core.windows.net/";
			public static string Key = "zyr2j7kSfhuf3BxySWXTMrpzlNUO4YFl6+kOIaD4uHJKK1jWV9aQr4gzx7eVJ33auScvc49vRhtQcgIMjlq0rA==";
			public static string ContainerName = "dotnet3";
			public static string FileLocation = BlobEndPoint + ContainerName;
		}

		public static async Task<bool> deleteFromBlobStorage_async(string fileName)
		{

			string containerName = AzureStorageConstants.ContainerName ;
			return await DeleteBlob_async(containerName, fileName);
		}

		private static async Task<bool> DeleteBlob_async(String containerName, String blobName)
		{
			String requestMethod = "DELETE";

			const String blobType = "BlockBlob";

			String urlPath = String.Format("{0}/{1}", containerName, blobName);
			String msVersion = "2009-09-19";
			String dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

			String canonicalizedHeaders = String.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
			String canonicalizedResource = String.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
			String stringToSign = String.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, 0, canonicalizedHeaders, canonicalizedResource);
			Debug.WriteLine("StringToSign=" + stringToSign);
			String authorizationHeader = CreateAuthorizationHeader(stringToSign);
			Debug.WriteLine("Authorization Header=" + authorizationHeader);

			string uri = AzureStorageConstants.BlobEndPoint + urlPath;
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
			client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
			client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
			client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
			//logRequest(requestContent, uri);

			HttpResponseMessage response = await client.DeleteAsync(uri);
			Debug.WriteLine("sent request");
			if (response.IsSuccessStatusCode == true) return true;

			Debug.WriteLine("Response =/n" + response.ToString());
			return false;

		}

		public static async Task<string> uploadToBlobStorage_async(Byte[] blobContent, string fileName)
		{

			string containerName = AzureStorageConstants.ContainerName;

			string Etag = await PutBlob_async(containerName, fileName, blobContent);

			PutQueue_async (fileName);

			return Etag;
		}

		private static async Task PutQueue_async(string fileName)
		{
			try
			{
				JToken payload = JObject.FromObject( new { filename = fileName });

				var resultJson = await App.ServiceClient.InvokeApiAsync("queue", payload);

				Debug.Assert(resultJson.Value<string>("filename") == fileName);
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}
		}

		private static async Task<string> PutBlob_async(String containerName, String blobName, Byte[] blobContent)
		{
			try
			{
				String requestMethod = "PUT";

				//String content = "The Name of This Band is Talking Heads";
				//UTF8Encoding utf8Encoding = new UTF8Encoding();
				//Byte[] blobContent = utf8Encoding.GetBytes(content);
				Int32 blobLength = blobContent.Length;

				const String blobType = "BlockBlob";

				String urlPath = String.Format("{0}/{1}", containerName, blobName);
				String msVersion = "2009-09-19";
				String dateInRfc1123Format = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);

				String canonicalizedHeaders = String.Format("x-ms-blob-type:{0}\nx-ms-date:{1}\nx-ms-version:{2}", blobType, dateInRfc1123Format, msVersion);
				String canonicalizedResource = String.Format("/{0}/{1}", AzureStorageConstants.Account, urlPath);
				String stringToSign = String.Format("{0}\n\n\n{1}\n\n\n\n\n\n\n\n\n{2}\n{3}", requestMethod, blobLength, canonicalizedHeaders, canonicalizedResource);
				Debug.WriteLine("StringToSign=" + stringToSign);
				String authorizationHeader = CreateAuthorizationHeader(stringToSign);
				Debug.WriteLine("Authorization Header=" + authorizationHeader);

				string uri = AzureStorageConstants.BlobEndPoint + urlPath;
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);
				client.DefaultRequestHeaders.Add("x-ms-date", dateInRfc1123Format);
				client.DefaultRequestHeaders.Add("x-ms-version", msVersion);
				Debug.WriteLine("Added all headers except authorisation");
				client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
				Debug.WriteLine("Added authorisation header");

				Debug.WriteLine("created new http client");
				HttpContent requestContent = new ByteArrayContent(blobContent);

				logRequest(requestContent, uri);

				HttpResponseMessage response = await client.PutAsync(uri, requestContent);
				Debug.WriteLine("sent request");
				if (response.IsSuccessStatusCode == true)
				{
					foreach (var aHeader in response.Headers)
					{
						if (aHeader.Key == "ETag") return aHeader.Value.ElementAt(0);
					}
				}
				Debug.WriteLine("Response =/n" + response.ToString());
			}
			catch (Exception ex)
			{
				string str = ex.Message;
			}

			return null;
		}

		private static String CreateAuthorizationHeader(String canonicalizedString)
		{
			if (String.IsNullOrEmpty(canonicalizedString))
			{
				throw new ArgumentNullException("canonicalizedString");
			}

			String signature = CreateHmacSignature(canonicalizedString, Convert.FromBase64String(AzureStorageConstants.Key));
			String authorizationHeader = String.Format(CultureInfo.InvariantCulture, "{0} {1}:{2}", AzureStorageConstants.SharedKeyAuthorizationScheme, AzureStorageConstants.Account, signature);

			return authorizationHeader;
		}

		private static String CreateHmacSignature(String unsignedString, Byte[] key)
		{
			if (String.IsNullOrEmpty(unsignedString))
			{
				throw new ArgumentNullException("unsignedString");
			}

			if (key == null)
			{
				throw new ArgumentNullException("key");
			}

			Byte[] dataToHmac = System.Text.Encoding.UTF8.GetBytes(unsignedString);
			using (HMACSHA256 hmacSha256 = new HMACSHA256(key))
			{
				return Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
			}
		}
		private static async void logRequest(HttpContent content, string requestURI)
		{
			Debug.WriteLine("RequestURI: " + requestURI);
			foreach (var aHeader in content.Headers)
			{
				Debug.WriteLine("Header: " + aHeader.Key + " : " + aHeader.Value.ToString());
			}

			Task<string> getContent = content.ReadAsStringAsync();
			Debug.WriteLine("Content:" + await getContent);

		}
	}
}
