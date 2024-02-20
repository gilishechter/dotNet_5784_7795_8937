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
using System.Windows.Shapes;

namespace PL.task
{
    /// <summary>
    /// Interaction logic for DepTaskWindow.xaml
    /// </summary>
    public partial class DepTaskWindow : Window
    {
        static int _Id;
        static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();
        public DepTaskWindow(int id)
        {
            InitializeComponent();
            _Id = id;
        }
        public int idDep
        {
            get { return (int)GetValue(idDepProperty); }
            set { SetValue(idDepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for idDep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty idDepProperty =
            DependencyProperty.Register("idDep", typeof(int), typeof(DepTaskWindow), new PropertyMetadata(0));


        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_ok(object sender, RoutedEventArgs e)
        {
            
            _s_bl.TaskList.Create(_Id, idDep);
            this.Close();
            MessageBox.Show("The task succesfully added", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
