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
using System.Windows.Shapes;

namespace PL.task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();
        }



        public IEnumerable<BO.TaskList> tasklist
        {
            get { return (IEnumerable<BO.TaskList>)GetValue(tasklistProperty); }
            set { SetValue(tasklistProperty, value); }
        }

        // Using a DependencyProperty as the backing store for tasklist.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tasklistProperty =
            DependencyProperty.Register("tasklist", typeof(IEnumerable<BO.TaskList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.filter filter { get; set; } = BO.filter.None;
        public BO.Status status { get; set; } = BO.Status.None;
        public BO.level level { get; set; } = BO.level.None;
        //private void ComboBox_SelectionChanged_filter(object sender, SelectionChangedEventArgs e)
        //{
        //    if(filter == BO.filter.ByStatus)
        //    {
        //        options.ItemsSource = Enum.GetValues(typeof(BO.Status));
        //    }
        //    else
        //    {
        //        options.ItemsSource = Enum.GetValues(typeof(BO.level));
        //    }

        //}

        private void Button_Click_AddTask(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            this.Close();   
        }

        private void double_click_updateWorker(object sender, MouseButtonEventArgs e)
        {

            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            new WorkerWindow(task.Id).ShowDialog();
            this.Close();
        }

        //private void options_SelectionChanged_filters(object sender, SelectionChangedEventArgs e)
        //{
        //    if (filter == BO.filter.None)
        //        s_bl?.Task.ReadAll();
        //    if (filter == BO.filter.ByStatus)
        //        s_bl.Task.ReadAll(t => t.Status == status);
        //    else
        //    {

        //    }


        //}
    }
}
