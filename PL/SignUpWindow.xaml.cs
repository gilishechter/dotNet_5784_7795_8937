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
        //BO.Worker ?worker1;
        //string? Name;
        //int Id;



        public BO.Worker worker
        {
            get { return (BO.Worker)GetValue(workerProperty); }
            set { SetValue(workerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty workerProperty =
            DependencyProperty.Register("worker", typeof(BO.Worker), typeof(SignUpWindow), new PropertyMetadata(null));


        public SignUpWindow(BO.Worker ?_worker=null)
        {
            //worker1=_worker;    
            //Name = _worker.Name;
            //Id = _worker.Id;
            worker = _worker;
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
                    BO.User user = new() { Id = passwordBox.Text, userName = usernameTextBox.Text, isAdmin = false };
                    if (worker == null)
                    {
                        _s_bl.User.Create(user);
                    }
                    else
                        _s_bl.User.Update(user);
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
