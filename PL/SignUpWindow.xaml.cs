using Accessibility;
using BO;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    /// 

    public partial class SignUpWindow : Window
    {

        static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();

        public string _name
        {
            get { return (string)GetValue(_nameProperty); }
            set { SetValue(_nameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _nameProperty =
            DependencyProperty.Register("_name", typeof(string), typeof(SignUpWindow), new PropertyMetadata(null));


        public int _id
        {
            get { return (int)GetValue(_idProperty); }
            set { SetValue(_idProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _id.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _idProperty =
            DependencyProperty.Register("_id", typeof(int), typeof(SignUpWindow), new PropertyMetadata(0));



        public bool isCreate
        {
            get { return (bool)GetValue(isUpdateProperty); }
            set { SetValue(isUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isUpdate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isUpdateProperty =
            DependencyProperty.Register("isUpdate", typeof(bool), typeof(SignUpWindow), new PropertyMetadata(true));

        public SignUpWindow(BO.Worker ?_worker=null)
        {
            if(_worker != null)
            {               
                _id = _worker.Id;
                _name = _worker.Name;
                isCreate = false;
            }
            //worker1=_worker;    
            //Name = _worker.Name;
            //Id = _worker.Id;
            
            InitializeComponent();
        }

        private void Button_Click_Cont(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid parentGrid = (Grid)button.Parent;
            
            TextBox nameTextBox = (TextBox)parentGrid.Children[4];
            TextBox IdBox = (TextBox)parentGrid.Children[5];

            TextBox usernameTextBox = (TextBox)parentGrid.Children[6];
            TextBox passwordBox = (TextBox)parentGrid.Children[7];
           // Name = nameTextBox.Text;
            int id = int.Parse(IdBox.Text);
           // Id = id;          
            try
            {               
                BO.Worker worker1 = _s_bl.Worker.Read(id)!;

                if (worker1!.Name != nameTextBox.Text)
                {
                    MessageBox.Show("This worker dosen't exist", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    BO.User user = new() { password = passwordBox.Text, userName = usernameTextBox.Text, isAdmin = false ,Id=id};
                    if (isCreate)
                    {
                        _s_bl.User.Create(user);
                        MessageBox.Show("This user signed up successfully", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        _s_bl.User.Update(user);
                        MessageBox.Show("This user successfully updated", "Well Done!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    this.Close();
                }

               
            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
