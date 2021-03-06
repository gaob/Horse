﻿using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace App
{
	/// <summary>
	/// View Model to add horses.
	/// </summary>
	public class HorseViewModel : BaseViewModel
	{
		private string Id { get; set;}
		private string owner_id { get; set; }
		private bool loaded { get; set; }
		private HorseItem horse { get; set; }
		private byte[] imageBytes { get; set; }

		public HorseViewModel (string the_owner_id, string theID)
		{
			Title = "Stable";
			Icon = "ic_play.png";
			Id = theID;
			owner_id = the_owner_id;
			loaded = false;
			horse = null;
			imageBytes = null;

			this.SaveCommand = new Command (async (nothing) => {
				// horse == null means it's an ADD action.
				if (horse == null) {
					horse = new HorseItem();
					horse.Name = Name;
					horse.Owner_id = owner_id;
					horse.Gender = Gender;
					horse.Year = Year;
					horse.Breed = Breed;
					horse.Registered = Registered;
					horse.Description = Description;

					IMobileServiceTable<HorseItem> HorseTable = App.ServiceClient.GetTable<HorseItem> ();

					await HorseTable.InsertAsync(horse);

					Id = horse.Id;

					MeVM.Horse_id = Id;
					MeVM.Horse_name = Name;

					if (imageBytes != null) {
						#pragma warning disable 4014
						// Uploading image doesn't need to be awaited in order to reduce client response time.
						RemoteBlobAccess.uploadToBlobStorage_async(imageBytes, "horse-" + Id + ".jpg");
						#pragma warning restore 4014
					}

					Error = "Added";
				} else {
					// horse != null means it's an UPDATE action.
					horse.Name = Name;
					horse.Owner_id = owner_id;
					horse.Gender = Gender;
					horse.Year = Year;
					horse.Breed = Breed;
					horse.Registered = Registered;
					horse.Description = Description;

					IMobileServiceTable<HorseItem> HorseTable = App.ServiceClient.GetTable<HorseItem> ();

					await HorseTable.UpdateAsync(horse);
					MeVM.Horse_name = Name;

					if (imageBytes != null) {
						#pragma warning disable 4014
						// Uploading image doesn't need to be awaited in order to reduce client response time.
						RemoteBlobAccess.uploadToBlobStorage_async(imageBytes, "horse-" + Id + ".jpg");
						#pragma warning restore 4014
					}

					Error = "Saved";
				}
			});
		}

		public void passImageBytes(byte[] theImageBytes)
		{
			imageBytes = theImageBytes;
		}

		private string name = string.Empty;

		public string Name
		{
			get { return name; }
			set { SetProperty (ref name, value, "Name");}
		}

		private string pic_url = string.Empty;

		public string Pic_url
		{
			get { return pic_url; }
			set { SetProperty (ref pic_url, value, "Pic_url");}
		}

		private string gender = string.Empty;

		public string Gender
		{
			get { return gender; }
			set { SetProperty (ref gender, value, "Gender");}
		}

		private int year = 0;

		public int Year
		{
			get { return year; }
			set { SetProperty (ref year, value, "Year");}
		}

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

		public ICommand SaveCommand { protected set; get; }

		/// <summary>
		/// Loads the horse values from the backend.
		/// </summary>
		/// <returns>The values.</returns>
		public async Task LoadValues()
		{
			if (IsBusy || loaded)
				return;

			IsBusy = true;

			try {
				IMobileServiceTable<HorseItem> HorseTable = App.ServiceClient.GetTable<HorseItem> ();

				if (Id != string.Empty) {
					horse = await HorseTable.LookupAsync(Id);
				}

				if (horse != null) {
					Name = horse.Name;
					Gender = horse.Gender;
					Year = horse.Year;
					Breed = horse.Breed;
					Registered = horse.Registered;
					Description = horse.Description;
					Pic_url = horse.Pic_url;
				} else {
					Name = string.Empty;
					Gender = string.Empty;
					Year = DateTime.Now.Year;
					Breed = string.Empty;
					Registered = string.Empty;
					Description = string.Empty;
					Pic_url = "http://rlv.zcache.co.uk/cartoon_norwegian_fjord_horse_cake_topper-r63a7a1509eae49d9a9203627eb8ea1c8_fupmp_8byvr_324.jpg";
				}

				loaded = true;
			} catch (Exception ex) {
				Error = ex.Message;
			}

			IsBusy = false;
		}
	}
}
