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
        BO.Worker ?worker1;
        public SignUpWindow(BO.Worker ?_worker=null)
        {
            worker1=_worker;    
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

            int id = int.Parse(IdBox.Text);
            try
            {
                if (worker1 == null) 
                {  
                BO.Worker worker = _s_bl.Worker.Read(id)!;

                if (worker!.Name != nameTextBox.Text)
                {
                    MessageBox.Show("This worker dosen't exist", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    BO.User user = new() { Id = passwordBox.Text, userName = usernameTextBox.Text, isAdmin = false };
                    _s_bl.User.Create(user);
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
