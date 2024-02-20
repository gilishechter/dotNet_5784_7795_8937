using PL.Tools.NewFolder;
using PL.Tools.ToObservableCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace PL.worker;

/// <summary>
/// Interaction logic for WorkerWindow.xaml
/// </summary>
public partial class WorkerlistWindow : Window
{
    static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();

    //private ObservableCollection<BO.Worker> _workers = new ObservableCollection<BO.Worker>();
    public WorkerlistWindow()
    {
        InitializeComponent();
    }

    private ObservableCollection<BO.Worker> _workers
    {
        get { return (ObservableCollection<BO.Worker>)GetValue(_workersProperty); }
        set { SetValue(_workersProperty, value); }
    }

    // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
    private static readonly DependencyProperty _workersProperty =
        DependencyProperty.Register("_workers", typeof(ObservableCollection<BO.Worker>), typeof(WorkerlistWindow), new PropertyMetadata(null));

    public BO.Rank Rank { get; set; } = BO.Rank.None;

    private void ComboBox_SelectionChanged_Rank(object sender, SelectionChangedEventArgs e)
    {
        _workers =  (Rank == BO.Rank.None) ?
        (_s_bl?.Worker.ReadAll()!).ToObservableCollection() : (_s_bl?.Worker.ReadAll(item => item.WorkerRank == Rank)!).ToObservableCollection()!;
    }

    private void Button_Click_addWorker(object sender, RoutedEventArgs e)
    {
        new WorkerWindow(onAddOrUpdate).ShowDialog();
    }

    private void Double_Click_Update(object sender, MouseButtonEventArgs e)
    {
        BO.Worker? worker =(sender as ListView)?.SelectedItem as BO.Worker;
        new WorkerWindow(onAddOrUpdate, worker.Id).ShowDialog();
      
    }

    private void onAddOrUpdate(int id, bool isUpdate)
    {
        if (isUpdate)
        {
            var oldWorker= _workers.FirstOrDefault(worker=> worker.Id == id);
            _workers.Remove(oldWorker);           
        }
        _workers.Add(_s_bl?.Worker.Read(id)!);///
    }
}

