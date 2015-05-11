using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace App
{
	/// <summary>
	/// Horse item class
	/// </summary>
	public class HorseItem
	{
		[JsonProperty(PropertyName = "Id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "Name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "Pic_url")]
		public string Pic_url { get; set; }
		[JsonProperty(PropertyName = "Owner_id")]
		public string Owner_id { get; set; }
		[JsonProperty(PropertyName = "Gender")]
		public string Gender { get; set; }
		[JsonProperty(PropertyName = "Year")]
		public int Year { get; set; }
		[JsonProperty(PropertyName = "Breed")]
		public string Breed { get; set; }
		[JsonProperty(PropertyName = "Registered")]
		public string Registered { get; set; }
		[JsonProperty(PropertyName = "Description")]
		public string Description { get; set; }

		/*
		public HorseItem (JObject theObject)
		{
			Name = "New Horse";
			Pic_url = "url";
			Owner_id = "1";
			Gender = "Male";
			Year = 1998;
			Breed = "Breed";
			Registered = "HA";
			Description = "No";
		}
		*/
	}
}
