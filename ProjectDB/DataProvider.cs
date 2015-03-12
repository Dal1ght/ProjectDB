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
				throw new SQLiteException();
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
				throw new SQLiteException();
			}
		}
		public void AddDeal(Deal deal)
		{
			// Добавление сделки в БД
			using (SQLiteCommand cmd = conn.CreateCommand())
			{
				cmd.CommandText = "INSERT INTO Deals(CustomerID, CarID, DealDate, ReturnDate, TotalPrice) "
								+ "VALUES (@customer, @car, @dealdate, @retdate, @totalp)";
				cmd.Parameters.AddWithValue("@customer", deal.Customer.ID);
				cmd.Parameters.AddWithValue("@car", deal.Car.ID);
				cmd.Parameters.AddWithValue("@dealdate", deal.DealDate);
				cmd.Parameters.AddWithValue("@retdate", deal.ReturnDate);
				cmd.Parameters.AddWithValue("@totalp", deal.TotalPrice);
				int ret = cmd.ExecuteNonQuery();
				if (ret != 1)
				{
					throw new SQLiteException("Insert error");
				}
			}
			// Расчет скидки
			using (SQLiteCommand cmd = conn.CreateCommand())
			{
				cmd.CommandText = "Select Count(*) from Deals where CustomerID = @customer";
				cmd.Parameters.AddWithValue("@customer", deal.Customer.ID);
				Int64 cnt = (Int64)cmd.ExecuteScalar();
				double discount;
				if (cnt < 25)
				{
					if (cnt < 10)
					{
						if (cnt < 3)
						{
							discount = 0.03;
						}
						else
						{
							discount = 0.05;
						}
					}
					else
					{
						discount = 0.1;
					}
				}
				else
				{
					discount = 0.17;
				}
				using (SQLiteCommand cmd2 = conn.CreateCommand())
				{
					cmd2.CommandText = "UPDATE Customers SET Discount = @disc WHERE ID = @id";
					cmd2.Parameters.AddWithValue("@id", deal.Customer.ID);
					cmd2.Parameters.AddWithValue("@disc", discount);
					int ret = cmd2.ExecuteNonQuery();
					if (ret != 1)
					{
						throw new SQLiteException("Discount error");
					}
				}
			}
		}

		public Car GetCar(Int64 id)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "Select * from Cars where ID = @id";
			cmd.Parameters.AddWithValue("@id", id);
			SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			DataRow dr = ds.Tables[0].Rows[0];
			Car c = new Car();
			c.ID = (Int64)dr[0];
			c.Brand = (string)dr[1];
			c.Cost = (Int64)dr[2];
			c.Type = (string)dr[3];
			c.BuildYear = (Int64)dr[4];
			return c;
		}
		public Customer GetCustomer(Int64 id)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "Select * from Customers where ID = @id";
			cmd.Parameters.AddWithValue("@id", id);
			SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			DataRow dr = ds.Tables[0].Rows[0];
			Customer c = new Customer();
			c.ID = (Int64)dr[0];
			c.LastName = (string)dr[1];
			c.FirstName = (string)dr[2];
			c.MiddleName = (string)dr[3];
			c.Address = (string)dr[4];
			c.Phone = (string)dr[5];
			c.Discount = (double)dr[6];
			return c;
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
		public List<Deal> GetDeals()
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "Select * from Deals";
			SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
			DataSet ds = new DataSet();
			adp.Fill(ds);
			List<Deal> deals = new List<Deal>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				Deal d = new Deal();
				d.ID = (Int64)dr[0];
				Int64 cusid = (Int64)dr[1];
				Int64 carid = (Int64)dr[2];
				d.Car = GetCar(carid);
				d.Customer = GetCustomer(cusid);
				d.DealDate = (DateTime)dr[3];
				d.ReturnDate = (DateTime)dr[4];
				d.TotalPrice = (Int64)dr[5];
				deals.Add(d);
			}
			return deals;
		}

	}
}
