using PL.Tools.ToObservableCollection;
using PL.Tools.NewFolder;
using PL.worker;
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

namespace PL.task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();
    public TaskListWindow()
    {
        InitializeComponent();
    }



    private ObservableCollection<BO.TaskList> _tasks
    {
        get { return (ObservableCollection<BO.TaskList>)GetValue(tasklistProperty); }
        set { SetValue(tasklistProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _tasks.  This enables animation, styling, binding, etc...
    private static readonly DependencyProperty tasklistProperty =
        DependencyProperty.Register("_tasks", typeof(ObservableCollection<BO.TaskList>), typeof(TaskListWindow), new PropertyMetadata(null));

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
        new TaskWindow(onAddOrUpdate).ShowDialog();
    }
    private void double_click_updateTask(object sender, MouseButtonEventArgs e)
    {
        BO.TaskList? task = (sender as ListView)?.SelectedItem as BO.TaskList;
        new TaskWindow(onAddOrUpdate,task.Id).ShowDialog();
    }

    private void ComboBox_SelectionChanged_status(object sender, SelectionChangedEventArgs e)
    {
        _tasks =( (status == BO.Status.None) ?
       _s_bl?.Task.ReadAll()! : _s_bl?.Task.ReadAll(item => item.Status == status)!).ToObservableCollection();
    }

    private void onAddOrUpdate(int id, bool isUpdate)
    {
        BO.TaskList tasklist = new BO.TaskList()
        {
            Id = id,
            Status = _s_bl.Task.Read(id)!.Status,
            Name = _s_bl?.Task.Read(id)!.Name,
            Description = _s_bl?.Task.Read(id)!.Description
        };
        if (isUpdate)
        {
            var oldTask = _tasks.FirstOrDefault(task => task.Id == id);
            _tasks.Remove(oldTask!);
            _tasks.OrderBy(task => task.Id).ToObservableCollection();
            
        }           
        _tasks.Add(tasklist);
    }
  
    private void click_DeleteTask(object sender, RoutedEventArgs e)
    {
        try
        {
            MessageBoxResult result = MessageBox.Show("Are yoe sure you want do delete this task", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            BO.TaskList _task = (sender as Button)?.CommandParameter as BO.TaskList;

            if (MessageBoxResult.Yes == result)
            {
                _s_bl.Task.Delete(_task.Id);
                _tasks.Remove(_task!);
                MessageBox.Show("This task has been successfully deleted", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


}
