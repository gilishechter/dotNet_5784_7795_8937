using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PL.worker;

/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
public partial class WorkerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
   
    private event Action<int, bool> _onAddOrUpdate;

    private readonly bool _isUpdate;

    private BO.Worker? _worker
    {
        get { return (BO.Worker)GetValue(workerProperty); }
        set { SetValue(workerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _worker.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty workerProperty =
        DependencyProperty.Register("_worker", typeof(BO.Worker), typeof(WorkerWindow), new PropertyMetadata(null));

    public WorkerWindow(Action<int, bool> onAddOrUpdate, int id=0)
    {
        InitializeComponent();
        _onAddOrUpdate = onAddOrUpdate;
        try
        {
            _isUpdate = id is not 0;
            _worker = (_isUpdate ? s_bl.Worker.Read(id) : new BO.Worker());
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
            var content = (sender as Button)!.Content;

            if (_isUpdate) s_bl.Worker.Update(_worker!);

            else s_bl.Worker.Create(_worker!);
           
            _onAddOrUpdate(_worker!.Id, _isUpdate);

            if (content is "Add") content += "e";
            MessageBox.Show($"The worker has been successfully {content}d", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
