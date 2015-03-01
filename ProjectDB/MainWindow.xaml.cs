﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectDB
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private SQLiteConnection conn;
		private Customer customer;
		private List<Label> menuItems;
		private Label menuSelection = null;
		private Grid currentGrid = null;
		public MainWindow()
		{
			InitializeComponent();
			customer = new Customer();
			Grid1.DataContext = customer;
			menuItems = new List<Label>();
			foreach (UIElement el in (GridMenu.Children[0] as Grid).Children)
			{
				if (el is Label)
				{
					menuItems.Add(el as Label);
				}
			}
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			conn = new SQLiteConnection("Data Source=data.db");
			conn.Open();
			/*ThicknessAnimationUsingKeyFrames taukf = new ThicknessAnimationUsingKeyFrames();
			taukf.BeginTime = new TimeSpan(0, 0, 0, 0, 0);
			taukf.Duration = new Duration(TimeSpan.FromMilliseconds(2000));
			taukf.KeyFrames = new ThicknessKeyFrameCollection();
			taukf.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(Width, 10, -Width, 10), KeyTime.FromPercent(0)));
			taukf.KeyFrames.Add(new SplineThicknessKeyFrame(new Thickness(10, 10, 10, 10), KeyTime.FromPercent(1)));
			Grid1.BeginAnimation(MarginProperty, taukf);*/
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			conn.Close();
			conn.Dispose();
		}

		private void DoRegisterButton_Click(object sender, RoutedEventArgs e)
		{
			SQLiteCommand cmd = conn.CreateCommand();
			cmd.CommandText = "INSERT INTO Customers(FirstName, LastName, MiddleName, Address, Phone) "
  + "VALUES (@firstName, @lastName, @middleName, @address, @phone);";
			cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
			cmd.Parameters.AddWithValue("@lastName", customer.LastName);
			cmd.Parameters.AddWithValue("@middleName", customer.MiddleName);
			cmd.Parameters.AddWithValue("@address", customer.Address);
			cmd.Parameters.AddWithValue("@phone", customer.Phone);
			int ret = cmd.ExecuteNonQuery();
			if (ret == 1)
			{
				customer.Result = "Клиент зарегистрирован успешно!";
			}
			else
			{
				customer.Result = "Во время регистрации возникла ошибка";
			}
			BtnGoRegister.Visibility = System.Windows.Visibility.Hidden;
			TBRegisterResult.Visibility = System.Windows.Visibility.Visible;
		}

		/// <summary>
		/// Fadeout menu animation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_Click(object sender, MouseButtonEventArgs e)
		{
			Label item = sender as Label;
			Storyboard sb = new Storyboard();

			// Анимация выбранного элемента
			DoubleAnimation da1 = new DoubleAnimation()
			{
				From = 1.0,
				To = 0.4,
				Duration = new Duration(TimeSpan.FromMilliseconds(150)),
				AutoReverse = true,
				RepeatBehavior = new RepeatBehavior(3),
				FillBehavior = FillBehavior.Stop
			};
			Storyboard.SetTarget(da1, item);
			Storyboard.SetTargetProperty(da1, new PropertyPath(Label.OpacityProperty));

			DoubleAnimation da2 = new DoubleAnimation()
			{
				From = 0,
				To = -Width,
				BeginTime = TimeSpan.FromMilliseconds(900),
				Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
				FillBehavior = FillBehavior.Stop
			};
			Storyboard.SetTarget(da2, item);
			Storyboard.SetTargetProperty(da2, new PropertyPath("RenderTransform.Children[3].X"));

			DoubleAnimation da3 = new DoubleAnimation()
			{
				From = 1,
				To = 0,
				BeginTime = TimeSpan.FromMilliseconds(900),
				Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
				FillBehavior = FillBehavior.Stop
			};
			Storyboard.SetTarget(da3, item);
			Storyboard.SetTargetProperty(da3, new PropertyPath(Label.OpacityProperty));

			DoubleAnimation da4 = new DoubleAnimation()
			{
				From = 1,
				To = 0,
				BeginTime = TimeSpan.FromMilliseconds(1800),
				Duration = new Duration(TimeSpan.FromMilliseconds(100)),
				FillBehavior = FillBehavior.HoldEnd
			};
			Storyboard.SetTarget(da4, GridMenu);
			Storyboard.SetTargetProperty(da4, new PropertyPath(Grid.OpacityProperty));

			// Анимация остальных пунктов
			foreach (Label oitem in menuItems)
			{
				if (oitem != item)
				{
					DoubleAnimation dao1 = new DoubleAnimation()
					{
						From = 0,
						To = oitem.Margin.Top < item.Margin.Top ? -Height : Height,
						BeginTime = TimeSpan.FromMilliseconds(900),
						Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
						FillBehavior = FillBehavior.Stop
					};
					Storyboard.SetTarget(dao1, oitem);
					Storyboard.SetTargetProperty(dao1, new PropertyPath("RenderTransform.Children[3].Y"));

					DoubleAnimation dao2 = new DoubleAnimation()
					{
						From = 1,
						To = 0,
						BeginTime = TimeSpan.FromMilliseconds(900),
						Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
						FillBehavior = FillBehavior.Stop
					};
					Storyboard.SetTarget(dao2, oitem);
					Storyboard.SetTargetProperty(dao2, new PropertyPath(Label.OpacityProperty));

					sb.Children.Add(dao1);
					sb.Children.Add(dao2);
				}
			}

			// Выбор нужной таблицы
			menuSelection = item;
			if (menuSelection == MainMenu1)
			{
				// Menu Item 1
				currentGrid = Grid1;
			}
			else if (menuSelection == MainMenu2)
			{
				currentGrid = Grid2;
			}
			else if (menuSelection == MainMenu3)
			{
				currentGrid = Grid3;
			}
			else if (menuSelection == MainMenu4)
			{
				currentGrid = Grid4;
			}
			else if (menuSelection == MainMenu5)
			{
				currentGrid = Grid5;
			}

			// FadeIn новой таблицы
			DoubleAnimation dat1 = new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = new Duration(TimeSpan.FromMilliseconds(900)),
				BeginTime = TimeSpan.FromMilliseconds(1400),
			};
			Storyboard.SetTarget(dat1, currentGrid);
			Storyboard.SetTargetProperty(dat1, new PropertyPath("RenderTransform.Children[0].ScaleX"));

			DoubleAnimation dat2 = new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = new Duration(TimeSpan.FromMilliseconds(900)),
				BeginTime = TimeSpan.FromMilliseconds(1400),
			};
			Storyboard.SetTarget(dat2, currentGrid);
			Storyboard.SetTargetProperty(dat2, new PropertyPath("RenderTransform.Children[0].ScaleY"));

			//Применение анимаций
			sb.Children.Add(da1);
			sb.Children.Add(da2);
			sb.Children.Add(da3);
			sb.Children.Add(da4);
			sb.Children.Add(dat1);
			sb.Children.Add(dat2);
			sb.Completed += sb_Completed;

			currentGrid.Visibility = Visibility.Visible;
			currentGrid.IsEnabled = false;
			GridMenu.IsEnabled = false;
			sb.Begin(this);
		}

		void sb_Completed(object sender, EventArgs e)
		{
			GridMenu.Visibility = Visibility.Hidden;
			currentGrid.IsEnabled = true;
		}

		private void BackButton_Click(object sender, MouseButtonEventArgs e)
		{
			currentGrid.IsEnabled = false;
			GridMenu.Visibility = System.Windows.Visibility.Visible;

			Storyboard sb = new Storyboard();
			DoubleAnimation da1 = new DoubleAnimation()
			{
				From = -Width,
				To = 0,
				Duration = new Duration(TimeSpan.FromMilliseconds(1000))
			};
			Storyboard.SetTarget(da1, GridMenu);
			Storyboard.SetTargetProperty(da1, new PropertyPath("RenderTransform.Children[3].X"));

			DoubleAnimation da2 = new DoubleAnimation()
			{
				From = 0,
				To = 1,
				Duration = new Duration(TimeSpan.FromMilliseconds(1))
			};
			Storyboard.SetTarget(da2, GridMenu);
			Storyboard.SetTargetProperty(da2, new PropertyPath(Grid.OpacityProperty));

			DoubleAnimation da3 = new DoubleAnimation()
			{
				From = 0,
				To = Width,
				Duration = new Duration(TimeSpan.FromMilliseconds(1000))
			};
			Storyboard.SetTarget(da3, currentGrid);
			Storyboard.SetTargetProperty(da3, new PropertyPath("RenderTransform.Children[3].X"));

			sb.Children.Add(da1);
			sb.Children.Add(da2);
			sb.Children.Add(da3);
			sb.Begin(this);

			//currentGrid.Visibility = System.Windows.Visibility.Hidden;
			GridMenu.IsEnabled = true;
		}
	}
}
