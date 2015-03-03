using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDB
{
	class DataProvider : IDisposable
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

		public void AddCar(string Brand, string Type, int BuildYear, int Cost)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "INSERT INTO Cars(Brand, Cost, Type, BuildYear) "
							+ "VALUES (@brand, @type, @buildyear, @cost);";
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
	}
}
