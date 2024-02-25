﻿using BlApi;
using PL.task;
using PL.worker;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //readonly BO.User user;
        //private event Action<int, bool> _onUpdate;

        private void onUpdate(int id,bool _update)
        {
            _update = true;
        }
        public BO.User user
        {
            get { return (BO.User)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));


        //public bool isAdmin
        //{
        //    get { return (bool)GetValue(isAdminProperty); }
        //    set { SetValue(isAdminProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for isAdmin.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty isAdminProperty =
        //    DependencyProperty.Register("isAdmin", typeof(bool), typeof(MainWindow), new PropertyMetadata(0));


        public MainWindow(BO.User _user)
        {
            user = _user;
            InitializeComponent();
           
        }

        private void Button_Click_Workers(object sender, RoutedEventArgs e)
        {
            new WorkerlistWindow().Show();
        }

        private void Button_Click_init(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the date?","Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
                // DalTest.Initialization.Do();
                s_bl.InitializeDB();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset the date?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == result)
                // DalTest.Initialization.Do();
                s_bl.Clear();
        }

        private void Button_Click_Tasks(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();

        }

        private void Button_Click_CurrentTask(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(user.Id);
            BO.Worker worker = s_bl.Worker.Read(id);

            //_onUpdate = onUpdate;

            new TaskWindow(onUpdate, worker.WorkerTask.Id.Value).ShowDialog();
        }

        private void Button_Click_ChooseTask(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(user.Id);
            BO.Worker worker = s_bl.Worker.Read(id);
            new TaskListWindow(worker).ShowDialog();
        }

        private void Button_Click_StartDate(object sender, RoutedEventArgs e)
        {
            new StartDateWindow().ShowDialog();
        }
    }
}
