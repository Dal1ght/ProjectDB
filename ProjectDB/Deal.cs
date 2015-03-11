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
		private int customerid;
		private int carid;
		private DateTime dealdate;
		private DateTime returndate;
		private DateTime actualreturndate;
		private string result;

		public int CustomerID { get { return customerid; } set { customerid = value; NotifyPropertyChanged(); } }
		public int CarID { get { return carid; } set { carid = value; NotifyPropertyChanged(); } }
		public DateTime DealDate { get { return dealdate; } set { dealdate = value; NotifyPropertyChanged(); } }
		public DateTime ReturnlDate { get { return returndate; } set { returndate = value; NotifyPropertyChanged(); } }
		public DateTime ActualReturnDate { get { return actualreturndate; } set { actualreturndate = value; NotifyPropertyChanged(); } }
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
			customerid = 0;
			carid = 0;
			dealdate = DateTime.Now;
			returndate = DateTime.Now;
			actualreturndate = DateTime.Now;
		}
	}
}
