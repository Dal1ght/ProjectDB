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
	/// Логика взаимодействия для SelectCar.xaml
	/// </summary>
	public partial class SelectCar : Window,  INotifyPropertyChanged
	{
		private List<Car> cars;
		private DataProvider dataProvider;
		private Car selcar;

		public Car SelectedCar { get { return selcar; } set { selcar = value; NotifyPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public SelectCar()
		{
			InitializeComponent();
		}

		public SelectCar(DataProvider dp)
			: this ()
		{
			dataProvider = dp;
			cars = dataProvider.GetCars();
			CarsGrid.ItemsSource = cars;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedCar == null)
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
