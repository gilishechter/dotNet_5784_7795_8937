using PL.task;
using PL.Tools.ToObservableCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing.IndexedProperties;
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

namespace PL;

/// <summary>
/// Interaction logic for GanttChartWindow.xaml
/// </summary>
public partial class GanttChartWindow : Window
{
    static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();



    public int _width
    {
        get { return (int)GetValue(widthProperty); }
        set { SetValue(widthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for width.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty widthProperty =
        DependencyProperty.Register("width", typeof(int), typeof(GanttChartWindow), new PropertyMetadata(null));


    private ObservableCollection<BO.TaskList> _tasks
    {
        get { return (ObservableCollection<BO.TaskList>)GetValue(tasklistProperty); }
        set { SetValue(tasklistProperty, value); }

    }

    // Using a DependencyProperty as the backing store for _tasks.  This enables animation, styling, binding, etc...
    private static readonly DependencyProperty tasklistProperty =
        DependencyProperty.Register("_tasks", typeof(ObservableCollection<BO.TaskList>), typeof(TaskListWindow), new PropertyMetadata(null));






    public GanttChartWindow()
    {
    //   var _tasks =_s_bl.Task.ReadAll();
    //    foreach (var task in _tasks)
    //    {
    //       ListViewItem listItem = new ListViewItem();
    //        listItem.Content = task.Id;
    //        listItem.Width=

    //        var temp = _s_bl.Task.Read(task.Id);
            
    //        //_width =int.Parse((temp.EndingDate - temp.StartDate).ToString());

    //    }
        

        InitializeComponent();
        

    }




}
    

