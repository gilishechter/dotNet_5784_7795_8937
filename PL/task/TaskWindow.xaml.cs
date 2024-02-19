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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskWindow(int id=0)
        {
            InitializeComponent();
            try
            {
                if (id == 0)
                    task = new BO.Task();
                else
                    task = s_bl.Task.Read(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public BO.Task task
        {
            get { return (BO.Task)GetValue(taskProperty); }
            set { SetValue(taskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty taskProperty =
            DependencyProperty.Register("task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void Button_Click_AddOrUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                if (s_bl.Task.ReadAll().FirstOrDefault(tmp => tmp.Id == task.Id) == null)
                {
                    s_bl.Task.Create(task);
                    MessageBox.Show("The task has been successfully added", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    //new WorkerlistWindow().Show();
                }
                else
                {
                    s_bl.Task.Update(task);
                    MessageBox.Show("The task has been successfully updated", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                   // new WorkerlistWindow().Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    
}
