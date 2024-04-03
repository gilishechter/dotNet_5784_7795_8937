
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GanttChartWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.GanttDetails> TaskOfList
        {
            get { return (IEnumerable<BO.GanttDetails>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskOfList", typeof(IEnumerable<BO.GanttDetails>), typeof(GanttChartWindow), new PropertyMetadata(null));
        public GanttChartWindow()
        {
            InitializeComponent();
            TaskOfList = s_bl?.Task.GetDetailsToGantt();
        }

    }
}