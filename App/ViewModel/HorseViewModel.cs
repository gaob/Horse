using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace App
{
	public class HorseViewModel : BaseViewModel
	{
		private string Id { get; set;}
		private bool loaded { get; set; }

		public HorseViewModel (string theID)
		{
			Title = "Stable";
			Icon = "blog.png";
			Id = theID;
			loaded = false;
		}

		private string name = string.Empty;

		public string Name
		{
			get { return name; }
			set { SetProperty (ref name, value, "Name");}
		}

		private string gender = string.Empty;

		public string Gender
		{
			get { return gender; }
			set { SetProperty (ref gender, value, "Gender");}
		}

		//Add Year

		private string breed = string.Empty;

		public string Breed
		{
			get { return breed; }
			set { SetProperty (ref breed, value, "Breed");}
		}

		private string registered = string.Empty;

		public string Registered
		{
			get { return registered; }
			set { SetProperty (ref registered, value, "Registered");}
		}

		private string description = string.Empty;

		public string Description
		{
			get { return description; }
			set { SetProperty (ref description, value, "Description");}
		}

		private string error = string.Empty;

		public string Error
		{
			get { return error; }
			set { SetProperty (ref error, value, "Error");}
		}

		public async Task LoadValues()
		{
			if (IsBusy || loaded)
				return;

			IsBusy = true;

			try{
				IMobileServiceTable<HorseItem> HorseTable = App.ServiceClient.GetTable<HorseItem> ();

				HorseItem horse = await HorseTable.LookupAsync(Id);

				Name = horse.Name;
				Gender = horse.Gender;
				Breed = horse.Breed;
				Registered = horse.Registered;
				Description = horse.Description;

				loaded = true;
			} catch (Exception ex) {
				Error = ex.Message;
			}

			IsBusy = false;
		}
	}
}
