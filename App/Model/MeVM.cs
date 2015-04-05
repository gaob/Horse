using System;
using Newtonsoft.Json.Linq;

namespace App
{
	public class MeVM
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Pic_url { get; set; }

		public MeVM (JObject theObject)
		{
			Id = theObject.Value<string> ("id");
			Name = theObject.Value<string> ("name");
			Pic_url = theObject.Value<string> ("pic_url");
		}
	}
}
