using System;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for StartDateWindow.xaml
    /// </summary>
    public partial class StartDateWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public StartDateWindow()
        {
            StartDate = s_bl.GetStartProject();
            InitializeComponent();
        }

        public DateTime? StartDate
        {
            get { return (DateTime?)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime?), typeof(StartDateWindow), new PropertyMetadata(null));


        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_ok(object sender, RoutedEventArgs e)
        {
            if (s_bl.GetStartProject() == null)
            {
                DateTime? selectedDate = StartDate;
                s_bl.SetStartProject(selectedDate);

                MessageBox.Show("The date has been successfully updated", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("The project already has start date project", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            this.Close();
        }
    }
}
