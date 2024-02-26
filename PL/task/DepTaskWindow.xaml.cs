using PL.Tools.ToObservableCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            tasks = _s_bl?.Task.ReadAll().ToObservableCollection();
            DataContext = this;
        }
        //public int idDep
        //{
        //    get { return (int)GetValue(idDepProperty); }
        //    set { SetValue(idDepProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for idDep.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty idDepProperty =
        //    DependencyProperty.Register("idDep", typeof(int), typeof(DepTaskWindow), new PropertyMetadata(0));

        public ObservableCollection<BO.TaskList> tasks
        {
            get { return (ObservableCollection<BO.TaskList>)GetValue(tasklistProperty); }
            set { SetValue(tasklistProperty, value); }

        }

        // Using a DependencyProperty as the backing store for _tasks.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty tasklistProperty =
            DependencyProperty.Register("tasks", typeof(ObservableCollection<BO.TaskList>), typeof(TaskListWindow), new PropertyMetadata(null));



        public BO.TaskList selectedTask
        {
            get { return (BO.TaskList)GetValue(taskProperty); }
            set { SetValue(taskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty taskProperty =
            DependencyProperty.Register("selectedTask", typeof(BO.TaskList), typeof(DepTaskWindow), new PropertyMetadata(null));



        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_ok(object sender, RoutedEventArgs e)
        {
            try {
              //BO.Task _task = (sender as ComboBox)?.SelectedItem as BO.Task;

                _s_bl.Task.Read(selectedTask.Id);

                _s_bl.TaskList.Create(_Id, selectedTask.Id);
                this.Close();
                MessageBox.Show("The task succesfully added", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
