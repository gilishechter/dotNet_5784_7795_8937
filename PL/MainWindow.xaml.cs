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

        
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeroperty); }
            set { SetValue(CurrentTimeroperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeroperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

       

        private void onUpdate(int id, bool _update)
        {
            _update = true;
        }
        public BO.User user
        {
            get { return (BO.User)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("user", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));


        public bool _isAdmin
        {
            get { return (bool)GetValue(isAdminProperty); }
            set { SetValue(isAdminProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isAdmin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isAdminProperty =
            DependencyProperty.Register("_isAdmin", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));


        public MainWindow(BO.User _user)
        {
            user = _user;
            _isAdmin = _user.isAdmin;
            CurrentTime = s_bl.Clock;
            InitializeComponent();

        }

        private void Button_Click_Workers(object sender, RoutedEventArgs e)
        {
            new WorkerlistWindow().Show();
        }

        private void Button_Click_init(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize the date?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

            BO.Worker worker = s_bl.Worker.Read(user.Id)!;

            //_onUpdate = onUpdate;
            if (worker.WorkerTask!.Id == null)
                MessageBox.Show("This worker doesn't have current task", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                new TaskWindow(onUpdate, worker.WorkerTask.Id!.Value,false, false).ShowDialog();

        }

        private void Button_Click_ChooseTask(object sender, RoutedEventArgs e)
        {

            BO.Worker worker = s_bl.Worker.Read(user.Id)!;
            new TaskListWindow(worker).ShowDialog();
        }

        private void Button_Click_StartDate(object sender, RoutedEventArgs e)
        {
            new StartDateWindow().ShowDialog();
        }

        private void Button_Click_updateUser(object sender, RoutedEventArgs e)
        {
            BO.Worker worker = s_bl.Worker.Read(user.Id)!;
            new SignUpWindow(worker).ShowDialog();

        }

        private void Button_Click_logOut(object sender, RoutedEventArgs e)
        {
           
           
            this.Close();
        }

        private void Button_Click_oneHour(object sender, RoutedEventArgs e)
        {
            CurrentTime= s_bl.SetClockHour();
         
        }

        private void Button_Click_oneDay(object sender, RoutedEventArgs e)
        {
            CurrentTime=s_bl.SetClockDay();
        }

        private void Button_Click_oneYear(object sender, RoutedEventArgs e)
        {
            CurrentTime = s_bl.SetClockYear();
        }

        private void Button_Click_ClockReset(object sender, RoutedEventArgs e)
        {
            CurrentTime= s_bl.ResetClock();
        }

        private void Button_Click_Aoto(object sender, RoutedEventArgs e)
        {
            try
            {
                if(s_bl.GetStartProject()==null)
                    MessageBox.Show("You can't plan a schedule because there is no start project date ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                s_bl.AutometicSchedule();
                MessageBox.Show("The schdule successfully updated", "Well Done", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}


    
     
