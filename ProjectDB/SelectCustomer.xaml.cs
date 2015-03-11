using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectDB
{
	/// <summary>
	/// Логика взаимодействия для SelectCustomer.xaml
	/// </summary>
	public partial class SelectCustomer : Window,  INotifyPropertyChanged
	{
		private List<Customer> customers;
		private DataProvider dataProvider;
		private Customer selCust;

		public Customer SelectedCustomer { get { return selCust; } set { selCust = value; NotifyPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public SelectCustomer()
		{
			InitializeComponent();
		}

		public SelectCustomer(DataProvider dp)
			: this ()
		{
			dataProvider = dp;
			customers = dataProvider.GetCustomers();
			CustomersGrid.ItemsSource = customers;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedCustomer == null)
			{
				if (MessageBox.Show("Заказчик не выбран. Закрыть окно выбора?", "Продолжить", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					DialogResult = false;
					this.Close();
				}
			}
			else
			{
				DialogResult = true;
				this.Close();
			}
		}
	}
}
