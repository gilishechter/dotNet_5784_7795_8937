using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.worker;


/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
public partial class WorkerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private event Action<int, bool> _onAddOrUpdate;
    public BO.Worker worker
    {
        get { return (BO.Worker)GetValue(workerProperty); }
        set { SetValue(workerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for worker.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty workerProperty =
        DependencyProperty.Register("worker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));

    public WorkerWindow(Action<int, bool> onAddOrUpdate, int id=0)
    {
        InitializeComponent();
        _onAddOrUpdate = onAddOrUpdate;
        try
        {
            if (id == 0)
                worker = new BO.Worker();
            else
               worker= s_bl.Worker.Read(id);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Button_Click_AddOrUpdate(object sender, RoutedEventArgs e)
    {
        try
        {
            if (s_bl.Worker.ReadAll().FirstOrDefault(tmp=> tmp.Id ==worker.Id) == null)
            {  
                s_bl.Worker.Create(worker);
                _onAddOrUpdate(worker.Id, true);

                MessageBox.Show("The worker has been successfully added", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
               
            }
            else
            {
                s_bl.Worker.Update(worker);
                _onAddOrUpdate(worker.Id, false);

                MessageBox.Show("The worker has been successfully updated",  "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
