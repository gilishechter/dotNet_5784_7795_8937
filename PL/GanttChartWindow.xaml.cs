using BO;
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


    public GanttChartWindow() { 

         // יצירת Canvas
        Canvas canvas = new Canvas();
    canvas.Width = 800;
        canvas.Height = 400;

        // הוספת Canvas לתוך StackPanel ב-Window
        
      var tasks = _s_bl.Task.ReadAll();
        DrawGanttChart(tasks,canvas);
        InitializeComponent();
        MainStackPanel.Children.Add(canvas);




        // יצירת תרשים גנט

    }

    private void DrawGanttChart(IEnumerable<BO.TaskList> tasks,Canvas canvas)
    {
        // יצירת Canvas
       
        // רשימת צבעים לפי סטטוס
        Dictionary<Status, Brush> statusColors = new Dictionary<Status, Brush>
    {
        { Status.OnTrackStarted, Brushes.Yellow },
        { Status.Scheduled, Brushes.Blue },
        { Status.Unscheduled, Brushes.Green }
        // ניתן להוסיף עוד סטטוסים וצבעים כרצונך
    };
        int i = 0;
        // ציור מלבנים עבור כל משימה
        foreach (var _task in tasks)
        {
            var task = _s_bl.Task.Read(_task.Id);
            var taskFirst = _s_bl.Task.Read(tasks.First().Id);
            double x1 = 0;
            double x2 = 0;

            // בדיקה שהתאריכים אינם ריקים לפני שמבצעים פעולות עליהם
            //if (task.StartDate != null && task.EndingDate != null)
            //{
            //    DateTime ?min=taskFirst.StartDate;
            //    // חישוב תרשימי הקווים על המישור
            //    foreach(var _taskCheck in tasks)
            //    {
            //        var check = _s_bl.Task.Read(_taskCheck.Id).StartDate;
            //        if (check < min)
            //            min = check;
            //    }
               
            //}
            if (task.StartDate != null)
            {
                x1= (task.StartDate - _s_bl.GetStartProject()).Value.TotalDays;
                x1 = x1 * 40+10;
            }
            if (task.EndingDate != null)
            {
                x2 = (task.EndingDate - _s_bl.GetStartProject()).Value.TotalDays;
                x2 = x2 * 40+10;
                
            }

            double y = 20 + 50 * i; // מיקום בציר Y

            // צביעת המלבן בצבע המתאים לפי סטטוס המשימה
            //   Brush statusColor = statusColors[task.Status];
            Rectangle rectangle = new Rectangle
            {
               
                Width = x2 - x1,
                Height = 30, // גובה קבוע של המלבן

                Fill = Brushes.Yellow,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Canvas.SetLeft(rectangle, x1);
            Canvas.SetTop(rectangle, y);
            canvas.Children.Add(rectangle);

            i++;
        }
    }
}





