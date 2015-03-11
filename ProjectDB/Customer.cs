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
	public class Customer : IDataErrorInfo, INotifyPropertyChanged
	{
		private Int64 id;
		private string firstName;
		private string middleName;
		private string lastName;
		private string address;
		private string phone;
		private string result;
		private double discount;

		public Int64 ID { get { return id; } set { id = value; NotifyPropertyChanged(); Text = ""; } }
		public string FirstName { get { return firstName; } set { firstName = value; NotifyPropertyChanged(); Text = ""; } }
		public string MiddleName { get { return middleName; } set { middleName = value; NotifyPropertyChanged(); Text = ""; } }
		public string LastName { get { return lastName; } set { lastName = value; NotifyPropertyChanged(); Text = ""; } }
		public string Address { get { return address; } set { address = value; NotifyPropertyChanged(); } }
		public string Phone { get { return phone; } set { phone = value; NotifyPropertyChanged(); } }
		public string Result { get { return result; } set { result = value; NotifyPropertyChanged(); } }
		public double Discount { get { return discount; } set { discount = value; NotifyPropertyChanged(); } }
		public bool NoErrors
		{
			get
			{
				bool b;
				try
				{
					b = (this["FirstName"] == String.Empty) && (this["LastName"] == String.Empty) && (this["MiddleName"] == String.Empty) && (this["Address"] == String.Empty) && (this["Phone"] == String.Empty);
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
			get { return String.Empty; }
		}
		public string this[string columnName]
		{
			get
			{
				string err = String.Empty;
				switch (columnName)
				{
					case "FirstName":
						if (!String.IsNullOrEmpty(FirstName))
						{
							string fnpat = @"^([А-я\-])+$";
							if (!Regex.IsMatch(FirstName, fnpat))
							{
								err = "Поля Фамилия, Имя и Отчество могут содержать только буквы и знак дефиса ( - )";
							}
						}
						break;
					case "MiddleName":
						if (!String.IsNullOrEmpty(MiddleName))
						{
							string mnpat = @"^([А-я\-])+$";
							if (!Regex.IsMatch(MiddleName, mnpat))
							{
								err = "Поля Фамилия, Имя и Отчество могут содержать только буквы и знак дефиса ( - )";
							}
						}
						break;
					case "LastName":
						if (!String.IsNullOrEmpty(LastName))
						{
							string lnpat = @"^([А-я\-])+$";
							if (!Regex.IsMatch(LastName, lnpat))
							{
								err = "Поля Фамилия, Имя и Отчество могут содержать только буквы и знак дефиса ( - )";
							}
						}
						break;
					case "Address":
						if (!String.IsNullOrEmpty(Address))
						{
							string apat = @"^([А-я0-9\s,\./\-])+$";
							if (!Regex.IsMatch(Address, apat))
							{
								err = "Поле Адрес может содержать только буквы, цифры, пробелы и знаки . , /";
							}
						}
						break;
					case "Phone":
						if (!String.IsNullOrEmpty(Phone))
						{
							if (!Phone.StartsWith("+7") || Phone.Length != 12)
								err = "Номер телефона не соответствует формату (+7xxxxxxxxxx)";
						}
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

		public Customer()
		{
			FirstName = String.Empty;
			MiddleName = String.Empty;
			LastName = String.Empty;
			Address = String.Empty;
			Phone = String.Empty;
			Result = String.Empty;
		}

		public override string ToString()
		{
			if (ID != 0)
				return String.Format("{0} - {1} {2} {3}", ID, LastName, FirstName, MiddleName);
			else
				return String.Empty;
		}
	}
}
