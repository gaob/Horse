using System;
using Newtonsoft.Json.Linq;

namespace App
{
	public class MeVM
	{
		public string id { get; set; }
		public string name { get; set; }
		public string pic_url { get; set; }

		public MeVM (JObject theObject)
		{
			id = theObject.Value<string> ("id");
			name = theObject.Value<string> ("name");
			pic_url = theObject.Value<string> ("pic_url");
		}
	}
}
