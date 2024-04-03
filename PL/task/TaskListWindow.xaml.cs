using PL.Tools.ToObservableCollection;
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
    

    public bool isAllTasks
    {
        get { return (bool)GetValue(isAllTasksProperty); }
        set { SetValue(isAllTasksProperty, value); }
    }

    // Using a DependencyProperty as the backing store for isAllTasks.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty isAllTasksProperty =
        DependencyProperty.Register("isAllTasks", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(false));

    public bool isSignUpTask { get; set; } = false;
    int _idWorker = 0;
    public TaskListWindow(BO.Worker? worker=null, bool _isSignUpTask = true)
    {
       // DataContext=this;

        isSignUpTask = _isSignUpTask;
        if (worker != null)
        {
            if (_isSignUpTask)
                _tasks = _s_bl?.Task.OptionTasks(worker).ToObservableCollection();
            
            else
                _tasks = _s_bl?.Task.OptionSchduleTasks(worker).ToObservableCollection();
            isAllTasks = false;

            _idWorker = worker.Id;
        }
        else { isAllTasks = true; }
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

    
    public BO.Status status { get; set; } = BO.Status.None;
    public BO.level level { get; set; } = BO.level.None;
   
    private void Button_Click_AddTask(object sender, RoutedEventArgs e)
    {
        if (_s_bl.GetStartProject() != null)
            MessageBox.Show("you can't add task since the project startes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else
            new TaskWindow(onAddOrUpdate).ShowDialog();

    }
    private void double_click_updateTask(object sender, MouseButtonEventArgs e)
    {
        BO.TaskList? task = (sender as ListView)?.SelectedItem as BO.TaskList;
        new TaskWindow(onAddOrUpdate,task!.Id, isAllTasks, true, isSignUpTask, _idWorker).ShowDialog();
    }

    private void ComboBox_SelectionChanged_status(object sender, SelectionChangedEventArgs e)
    {
        if (isAllTasks)
        {
            _tasks = _s_bl.Task.ReadAll().ToObservableCollection();
            ObservableCollection<BO.TaskList> tempTaskList = new ObservableCollection<BO.TaskList>();
            if (status == BO.Status.None && level == BO.level.None)
            {
                return;
            }
            if (status == BO.Status.None)
            {

                foreach (var taskInList in _tasks)
                {
                    BO.Task tempTask = _s_bl.Task.Read(taskInList.Id)!;
                    if (tempTask.Rank == (int)level)
                        tempTaskList.Add(taskInList);

                }
                _tasks = tempTaskList;
                return;

            }
            if (level == BO.level.None)
            {
                _tasks = _s_bl.Task.ReadAll(t=>t.Status==status).ToObservableCollection();
                return;

            }
            foreach (var taskInList in _tasks)
            {
                BO.Task tempTask = _s_bl.Task.Read(taskInList.Id)!;
                if (tempTask.Rank == (int)level && taskInList.Status == status)
                    tempTaskList.Add(taskInList);

            }
            _tasks = tempTaskList;
        }
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
          
            
        }           
        _tasks.Add(tasklist);
        _tasks.OrderBy(task => task.Id).ToObservableCollection();
    }
  
    private void click_DeleteTask(object sender, RoutedEventArgs e)
    {
        try
        {
            MessageBoxResult result = MessageBox.Show("Are yoe sure you want do delete this task", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            BO.TaskList _task = (sender as Button)?.CommandParameter as BO.TaskList;

            if (MessageBoxResult.Yes == result)
            {
                _s_bl.Task.Delete(_task!.Id);
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
