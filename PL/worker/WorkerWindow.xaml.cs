using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
namespace PL.worker;

/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
public partial class WorkerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



    public BO.Worker worker
    {
        get { return (BO.Worker)GetValue(workerProperty); }
        set { SetValue(workerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for worker.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty workerProperty =
        DependencyProperty.Register("worker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));


    public WorkerWindow(int id=0)
    {
        InitializeComponent();
        try
        {
            if (id == 0)
                worker = new BO.Worker();
            else
                s_bl.Worker.Read(id);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
