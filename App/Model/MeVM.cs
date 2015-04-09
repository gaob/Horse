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
		public string Horse_id { get; set; }
		public string Horse_name { get; set; }

		public MeVM (JObject theObject)
		{
			Id = theObject.Value<string> ("id");
			Name = theObject.Value<string> ("name");
			Pic_url = theObject.Value<string> ("pic_url");
			Horse_id = theObject.Value<string> ("horse_id");
			Horse_name = theObject.Value<string> ("horse_name");
		}
	}
}
