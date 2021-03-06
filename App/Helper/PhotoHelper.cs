﻿using System;
using System.IO;

namespace App
{
	/// <summary>
	/// Photo helper to convert image stream to raw byte array.
	/// </summary>
	public class PhotoHelper
	{
		public static byte[] ReadFully(Stream input) {
			byte[] buffer = new byte[16*1024];
			using(MemoryStream ms = new MemoryStream()) {
				int read;
				while((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}
