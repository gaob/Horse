using System;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App
{
	public class MeVM
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Pic_url { get; set; }
		public string horse_id { get; set; }

		public MeVM (JObject theObject)
		{
			Id = theObject.Value<string> ("id");
			Name = theObject.Value<string> ("name");
			Pic_url = theObject.Value<string> ("pic_url");
			horse_id = theObject.Value<string> ("horse_id");
		}
	}
}
