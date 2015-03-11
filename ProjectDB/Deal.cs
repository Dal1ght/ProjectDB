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
	class Deal : IDataErrorInfo, INotifyPropertyChanged
	{
		private Customer customer;
		private Car car;
		private DateTime dealdate;
		private DateTime returndate;
		private DateTime actualreturndate;
		private string result;

		public Customer Customer { get { return customer; } set { customer = value; NotifyPropertyChanged(); } }
		public Car Car { get { return car; } set { car = value; NotifyPropertyChanged(); } }
		public DateTime DealDate { get { return dealdate; } set { dealdate = value; NotifyPropertyChanged(); } }
		public DateTime ReturnDate { get { return returndate; } set { returndate = value; NotifyPropertyChanged(); } }
		public DateTime ActualReturnDate { get { return actualreturndate; } set { actualreturndate = value; NotifyPropertyChanged(); } }
		public string Result { get { return result; } set { result = value; NotifyPropertyChanged(); } }
		public bool NoErrors { get {
			bool b;
			try
			{
				b = (Customer.ID != 0) && (Car.ID != 0);
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
					case "BuildYear":
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

		public Deal()
		{
			customer = new Customer();
			car = new Car();
			dealdate = DateTime.Now;
			returndate = DateTime.Now;
			actualreturndate = DateTime.Now;
		}
	}
}
