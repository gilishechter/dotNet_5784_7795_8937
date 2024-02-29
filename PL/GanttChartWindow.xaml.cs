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
    public ObservableCollection<BO.TaskList> task
    {
        get { return (ObservableCollection<BO.TaskList>)GetValue(taskopertyProperty); }
        set { SetValue(taskopertyProperty, value); }
    }

    // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty taskopertyProperty =
        DependencyProperty.Register("task", typeof(BO.Task), typeof(GanttChartWindow), new PropertyMetadata(null));


    public ObservableCollection<string>  Dates
    {
        get { return (ObservableCollection<string>)GetValue( DatesProperty); }
        set { SetValue( DatesProperty, value); }
    }

    // Using a DependencyProperty as the backing store for  Dates.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty  DatesProperty =
        DependencyProperty.Register(" Dates", typeof(ObservableCollection<string>), typeof(GanttChartWindow), new PropertyMetadata(0));



    public GanttChartWindow()
    {
        task = _s_bl.Task.ReadAll().ToObservableCollection();
        InitializeComponent();


    }




}
    

