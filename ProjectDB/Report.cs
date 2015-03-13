using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB
{
	public class Report : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public class CPeriod : INotifyPropertyChanged
		{
			private bool week;
			private bool month;
			private bool year;
			private bool alltime;

			public bool Week { get { return week; } set { week = value; NotifyPropertyChanged(); } }
			public bool Month { get { return month; } set { month = value; NotifyPropertyChanged(); } }
			public bool Year { get { return year; } set { year = value; NotifyPropertyChanged(); } }
			public bool AllTime { get { return alltime; } set { alltime = value; NotifyPropertyChanged(); } }
			public event PropertyChangedEventHandler PropertyChanged;
			private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			public CPeriod()
			{
				Week = false;
				Month = false;
				Year = false;
				AllTime = true;
			}
		}
		public class CFormat : INotifyPropertyChanged
		{
			private bool txt;
			private bool html;

			public bool TXT { get { return txt; } set { txt = value; NotifyPropertyChanged(); } }
			public bool HTML { get { return html; } set { html = value; NotifyPropertyChanged(); } }
			public event PropertyChangedEventHandler PropertyChanged;
			private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			public CFormat()
			{
				TXT = false;
				HTML = true;
			}
		}
		public Report()
		{
			Period = new CPeriod();
			Format = new CFormat();
		}
		public Report(DataProvider prov)
			: this()
		{
			dataprovider = prov;
		}
		private CPeriod period;
		private CFormat format;
		private List<Deal> deals;
		private List<Car> cars;
		private List<Customer> customers;
		private DataProvider dataprovider;

		public CPeriod Period { get { return period; } set { period = value; NotifyPropertyChanged(); } }
		public CFormat Format { get { return format; } set { format = value; NotifyPropertyChanged(); } }

		public void MakeReport()
		{
			cars = dataprovider.GetCars();
			customers = dataprovider.GetCustomers();
			deals = dataprovider.GetDeals();

			if (!Directory.Exists("Reports"))
			{
				Directory.CreateDirectory("Reports");
			}

			if (Format.TXT)
			{
				MakeReportTXT();
			}
			else if (Format.HTML)
			{
				MakeReportHTML();
			}
			else
			{
				throw new System.Exception();
			}
		}

		private void MakeReportTXT()
		{
			List<Deal> ldeals = GetDatedDeals();
			string frep = String.Format("Reports/report_{0:yyyy_MM_dd_HH_mm_ss}.txt", DateTime.Now);
			using (StreamWriter sw = File.CreateText(frep))
			{
				using (StreamReader sr = File.OpenText("Templates/report.txt"))
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();
						if (line.StartsWith("[CUSTOMER]"))
						{
							line = line.Substring(10);
							foreach (Customer c in customers)
							{
								string s = String.Format(line, c.ID, c.LastName, c.FirstName, c.MiddleName, c.Address, c.Phone, c.Discount);
								sw.WriteLine(s);
							}
						}
						else if (line.StartsWith("[CAR]"))
						{
							line = line.Substring(5);
							foreach (Car c in cars)
							{
								string s = String.Format(line, c.ID, c.Brand, c.Type, c.BuildYear, c.Cost);
								sw.WriteLine(s);
							}
						}
						else if (line.StartsWith("[DEAL]"))
						{
							line = line.Substring(6);
							foreach (Deal d in ldeals)
							{
								string s = String.Format(line, d.ID, d.Customer.Text, d.Car.Text, d.DealDate, d.ReturnDate, d.TotalPrice);
								sw.WriteLine(s);
							}
						}
						else
						{
							sw.WriteLine(line);
						}
					}
					sr.Close();
				}
				sw.Close();
			}
			Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/" + frep);
		}

		private void MakeReportHTML()
		{
			List<Deal> ldeals = GetDatedDeals();
			string frep = String.Format("Reports/report_{0:yyyy_MM_dd_HH_mm_ss}.html", DateTime.Now);
			using (StreamWriter sw = File.CreateText(frep))
			{
				using (StreamReader sr = File.OpenText("Templates/report.html"))
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine().Trim(new char[] { '\n', '\t', ' ' });
						if (line.StartsWith("[CUSTOMER]"))
						{
							line = line.Substring(10);
							foreach (Customer c in customers)
							{
								string s = String.Format(line, c.ID, c.LastName, c.FirstName, c.MiddleName, c.Address, c.Phone, c.Discount);
								sw.Write(s);
							}
						}
						else if (line.StartsWith("[CAR]"))
						{
							line = line.Substring(5);
							foreach (Car c in cars)
							{
								string s = String.Format(line, c.ID, c.Brand, c.Type, c.BuildYear, c.Cost);
								sw.Write(s);
							}
						}
						else if (line.StartsWith("[DEAL]"))
						{
							line = line.Substring(6);
							foreach (Deal d in ldeals)
							{
								string s = String.Format(line, d.ID, d.Customer.Text, d.Car.Text, d.DealDate, d.ReturnDate, d.TotalPrice);
								sw.Write(s);
							}
						}
						else
						{
							sw.Write(line);
						}
					}
					sr.Close();
				}
				sw.Close();
			}
			Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/" + frep);
		}

		private List<Deal> GetDatedDeals()
		{
			DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			DateTime sdate = now;
			List<Deal> ldeals;
			if (Period.AllTime)
			{
				ldeals = deals;
			}
			else
			{
				if (Period.Week)
					sdate = now.Subtract(TimeSpan.FromDays(7));
				else if (Period.Month)
					sdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
				else if (Period.Year)
					sdate = new DateTime(DateTime.Now.Year, 1, 1);
				else
					throw new NotImplementedException();
				ldeals = (from deal in deals where deal.DealDate > sdate select deal).ToList();
			}
			return ldeals;
		}
	}
}
