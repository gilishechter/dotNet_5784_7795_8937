using BlApi;
using PL.task;
using PL.worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Workers(object sender, RoutedEventArgs e)
        {
            new WorkerlistWindow().Show();
        }

        private void Button_Click_init(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the date?","Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
                // DalTest.Initialization.Do();
                s_bl.InitializeDB();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the date?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
                // DalTest.Initialization.Do();
                s_bl.Clear();
        }

        private void Button_Click_Tasks(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();

        }
    }
}
