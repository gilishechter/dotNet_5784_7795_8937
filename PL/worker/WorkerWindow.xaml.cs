﻿using System;
using System.Collections.Generic;
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

namespace PL.worker
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public WorkerWindow()
        {
            InitializeComponent();
            workers = s_bl?.Worker.ReadAll()!;
        }

        public IEnumerable<BO.Worker> workers
        {
            get { return (IEnumerable<BO.Worker>)GetValue(WorkersProperty); }
            set { SetValue(WorkersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkersProperty =
            DependencyProperty.Register("workers", typeof(IEnumerable<BO.Worker>), typeof(WorkerWindow), new PropertyMetadata(null));

        public BO.Rank Rank { get; set; } = BO.Rank.None;

        private void ComboBox_SelectionChanged_Rank(object sender, SelectionChangedEventArgs e)
        {
            workers = (Rank == BO.Rank.None) ?
            s_bl?.Worker.ReadAll()! : s_bl?.Worker.ReadAll(item => item.WorkerRank == Rank)!;
        }
    }
}
