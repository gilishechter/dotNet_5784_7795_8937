﻿using PL.Tools.ToObservableCollection;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private event Action<int, bool> _onAddOrUpdate;

        private readonly bool _isUpdate;

        private readonly int _id;



        public bool _isAllTasks { get; set; }=false;
        public bool _isCurrentTask { get; set; } = false;
        public TaskWindow(Action<int, bool> onAddOrUpdate, int id = 0, bool isAllTasks = true, bool isCurrentTask=true)
        {
            //InitializeComponent();
            DataContext = this;
            _onAddOrUpdate = onAddOrUpdate;
            _isAllTasks = isAllTasks;
           
           _id =id;
            try
            {
                _isUpdate = id is not 0;
                task = (_isUpdate ? s_bl.Task.Read(id) : new BO.Task())!;
                _isCurrentTask = isCurrentTask ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            InitializeComponent();
        }


        public ObservableCollection<BO.TaskList> depTasks
        {
            get { return (ObservableCollection<BO.TaskList>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("CurrentTime", typeof(ObservableCollection<BO.TaskList>), typeof(TaskWindow), new PropertyMetadata(null));


        public BO.StatusProject statusProject { get; set; } = BO.StatusProject.Planning;
        public BO.Task task
        {
            get { return (BO.Task)GetValue(taskProperty); }
            set { SetValue(taskProperty, value); }


        }

        // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty taskProperty =
            DependencyProperty.Register("task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void Button_Click_AddOrUpdateTask(object sender, RoutedEventArgs e)
        {
            try
            {
                var content = (sender as Button)!.Content;

                int _id = task.Id;

                if (_isUpdate) s_bl.Task.Update(task!);

                else _id = s_bl.Task.Create(task);

                _onAddOrUpdate(_id, _isUpdate);

                if (content is "Add") content += "e";

                MessageBox.Show($"The task has been successfully {content}d", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void double_click_updateDepTask(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Are yoe sure you want do delete this dependence task", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                BO.TaskList _task = (sender as Button)?.CommandParameter as BO.TaskList;

                if (MessageBoxResult.Yes == result)
                {
                    s_bl.TaskList.Delete(task.Id, _task.Id);
                    depTasks = s_bl.Task.Read(_task.Id).DependenceTasks.ToObservableCollection();

                    depTasks.Remove(_task!);
                    MessageBox.Show("This dependence task has been successfully deleted", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_AddDep(object sender, RoutedEventArgs e)
        {
            new DepTaskWindow(task.Id).ShowDialog();

        }

        private void reOpenList(object sender, EventArgs e)
        {
            task = (_isUpdate ? s_bl.Task.Read(_id) : new BO.Task())!;
        }
    }

}
