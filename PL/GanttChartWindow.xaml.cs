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

    private ObservableCollection<BO.TaskList> _tasks
    {
        get { return (ObservableCollection<BO.TaskList>)GetValue(tasklistProperty); }
        set { SetValue(tasklistProperty, value); }

    }

    // Using a DependencyProperty as the backing store for _tasks.  This enables animation, styling, binding, etc...
    private static readonly DependencyProperty tasklistProperty =
        DependencyProperty.Register("_tasks", typeof(ObservableCollection<BO.TaskList>), typeof(GanttChartWindow), new PropertyMetadata(null));



    public int _width
    {
        get { return (int)GetValue(_widthProperty); }
        set { SetValue(_widthProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _width.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _widthProperty =
        DependencyProperty.Register("_width", typeof(int), typeof(GanttChartWindow), new PropertyMetadata(null));


    public string color
    {
        get { return (string)GetValue(colorProperty); }
        set { SetValue(colorProperty, value); }
    }

    // Using a DependencyProperty as the backing store for color.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty colorProperty =
        DependencyProperty.Register("color", typeof(string), typeof(GanttChartWindow), new PropertyMetadata(null));



    public int _mergin
    {
        get { return (int)GetValue(_merginProperty); }
        set { SetValue(_merginProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _mergin.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _merginProperty =
        DependencyProperty.Register("_mergin", typeof(int), typeof(GanttChartWindow), new PropertyMetadata(null));



    public GanttChartWindow()
    {
       var _tasks =_s_bl.Task.ReadAll();
      
        foreach (var task in _tasks)
        {
            var temp = _s_bl.Task.Read(task.Id);
          //  ListBoxItem item = new ListBoxItem();
            if (temp != null)
            {
                //_width = int.Parse((temp.EndingDate - temp.StartDate).ToString());
                if (temp.Status == BO.Status.Unscheduled)
                    color = "blue";
                else
                    color = "red";
                


            }
        }
        InitializeComponent();
        

    }




}
    

