using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB
{
	public class DataProvider : IDisposable
	{
		private SQLiteConnection conn;

		public DataProvider()
		{
			conn = new SQLiteConnection("Data Source=data.db");
			conn.Open();
		}

		public void Dispose()
		{
			conn.Close();
			conn.Dispose();
		}

		public void AddCustomer(string FirstName, string LastName, string MiddleName, string Address, string Phone)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "INSERT INTO Customers(FirstName, LastName, MiddleName, Address, Phone) "
							+ "VALUES (@firstName, @lastName, @middleName, @address, @phone);";
			cmd.Parameters.AddWithValue("@firstName", FirstName);
			cmd.Parameters.AddWithValue("@lastName", LastName);
			cmd.Parameters.AddWithValue("@middleName", MiddleName);
			cmd.Parameters.AddWithValue("@address", Address);
			cmd.Parameters.AddWithValue("@phone", Phone);
			int ret = cmd.ExecuteNonQuery();
			if (ret != 1)
			{
				throw new System.Exception();
			}
		}

		public void AddCar(string Brand, string Type, Int64 BuildYear, Int64 Cost)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "INSERT INTO Cars(Brand, Cost, Type, BuildYear) "
							+ "VALUES (@brand, @cost, @type, @BuildYear);";
			cmd.Parameters.AddWithValue("@brand", Brand);
			cmd.Parameters.AddWithValue("@type", Type);
			cmd.Parameters.AddWithValue("@buildyear", BuildYear);
			cmd.Parameters.AddWithValue("@cost", Cost);
			int ret = cmd.ExecuteNonQuery();
			if (ret != 1)
			{
				throw new System.Exception();
			}
		}

		public List<Customer> GetCustomers()
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "Select * from Customers";
			SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			List<Customer> customers = new List<Customer>();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				Customer c = new Customer();
				c.ID = (Int64)dr[0];
				c.LastName = (string)dr[1];
				c.FirstName = (string)dr[2];
				c.MiddleName = (string)dr[3];
				c.Address = (string)dr[4];
				c.Phone = (string)dr[5];
				c.Discount = (double)dr[6];
				customers.Add(c);
			}
			return customers;
		}

		public List<Car> GetCars()
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "Select * from Cars";
			SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			List<Car> cars = new List<Car>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				Car c = new Car();
				c.ID = (Int64)dr[0];
				c.Brand = (string)dr[1];
				c.Cost = (Int64)dr[2];
				c.Type = (string)dr[3];
				c.BuildYear = (Int64)dr[4];
				cars.Add(c);
			}
			return cars;
		}

		public void MakeDeal(Deal deal)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "INSERT INTO Deals(CustomerID, CarID, DealDate, ReturnDate, TotalPrice) "
							+ "VALUES (@customer, @car, @dealdate, @retdate, @totalp)";
			cmd.Parameters.AddWithValue("@customer", deal.Customer.ID);
			cmd.Parameters.AddWithValue("@car", deal.Car.ID);
			cmd.Parameters.AddWithValue("@dealdate", deal.DealDate);
			cmd.Parameters.AddWithValue("@retdate", deal.ReturnDate);
			cmd.Parameters.AddWithValue("@totalp", deal.TotalPrice);
			int ret = cmd.ExecuteNonQuery();
			if(ret != 1)
			{
				throw new System.Exception();
			}
		}
	}
}
