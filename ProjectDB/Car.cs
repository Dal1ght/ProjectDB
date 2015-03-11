using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectDB
{
	public class Car : IDataErrorInfo, INotifyPropertyChanged
	{
		private Int64 id;
		private string barnd;
		private string type;
		private Int64 cost;
		private Int64 buildYear;
		private string result;

		public Int64 ID { get { return id; } set { id = value; NotifyPropertyChanged(); Text = ""; } }
		public string Brand { get { return barnd; } set { barnd = value; NotifyPropertyChanged(); Text = ""; } }
		public string Type { get { return type; } set { type = value; NotifyPropertyChanged(); Text = ""; } }
		public Int64 Cost { get { return cost; } set { cost = value; NotifyPropertyChanged();} }
		public Int64 BuildYear { get { return buildYear; } set { buildYear = value; NotifyPropertyChanged(); Text = ""; } }
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
						if (!String.IsNullOrEmpty(Brand))
						{
							string fnpat = @"^([А-яA-z\-])+$";
							if (!Regex.IsMatch(Brand, fnpat))
							{
								err = "Поле Марка может содержать только буквы и знак дефиса ( - )";
							}
						}
						break;
					case "Type":
						if (!String.IsNullOrEmpty(Type))
						{
							string mnpat = @"^([А-яA-z])+$";
							if (!Regex.IsMatch(Type, mnpat))
							{
								err = "Тип может содержать только буквы";
							}
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
		public string Text { get { return ToString(); } set { NotifyPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				if (propertyName != "NoErrors") NoErrors = true;
			}
		}

		public Car()
		{
			Brand = String.Empty;
			Type = String.Empty;
			Cost = 0;
			BuildYear = DateTime.Now.Year;
		}

		public override string ToString()
		{
			if (ID != 0)
				return String.Format("{0} - {1} {2} {3}", ID, Brand, Type, BuildYear);
			else
				return String.Empty;
		}
	}
}
