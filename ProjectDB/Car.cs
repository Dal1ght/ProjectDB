﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectDB
{
	class Car : IDataErrorInfo, INotifyPropertyChanged
	{
		private string barnd;
		private string type;
		private int cost;
		private int buildYear;
		private string result;

		public string Brand { get { return barnd; } set { barnd = value; NotifyPropertyChanged(); } }
		public string Type { get { return type; } set { type = value; NotifyPropertyChanged(); } }
		public int Cost { get { return cost; } set { cost = value; NotifyPropertyChanged(); } }
		public int BuildYear { get { return buildYear; } set { buildYear = value; NotifyPropertyChanged(); } }
		public string Result { get { return result; } set { result = value; NotifyPropertyChanged(); } }
		public bool NoErrors { get {
			bool b;
			try
			{
				b = (this["Brand"] == String.Empty) && (this["Type"] == String.Empty) && (this["Cost"] == String.Empty) && (this["BuildYear"] == String.Empty);
			}
			catch
			{
				b = false;
			}
			return b;
		}
			set { NotifyPropertyChanged(); }
		}
		public string Error
		{
			get { throw new NotImplementedException(); }
		}

		public string this[string columnName]
		{
			get 
			{
				string err = String.Empty;
				switch (columnName)
				{
					case "Brand":
						string fnpat = @"^([А-яA-z\-])+$";
						if (!Regex.IsMatch(Brand, fnpat))
						{
							err = "Поле Марка может содержать только буквы и знак дефиса ( - )";
						}
						break;
					case "Type":
						string mnpat = @"^([А-яA-z])+$";
						if (!Regex.IsMatch(Type, mnpat))
						{
							err = "Тип может содержать только буквы";
						}
						break;
					case "Cost":
						if (Cost < 0)
							err = "Стоимость не может быть отрицательной";
						break;
					case "BuildYear":
						if (BuildYear < 1950 || BuildYear > 2015)
							err = "Год выпуска может быть от 1950 до 2015";
						break;
				}
				return err;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				if (propertyName != "NoErrors") NoErrors = true;
			}
		}
	}
}