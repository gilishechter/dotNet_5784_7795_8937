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
using System.Windows.Threading;

namespace PL;

/// <summary>
/// Interaction logic for UserWindow.xaml
/// </summary>
public partial class UserWindow : Window
{
    static readonly BlApi.IBl _s_bl = BlApi.Factory.Get();
    public UserWindow()
    {
       // _s_bl.InitializeDB();
        InitializeComponent();
    }

    public BO.User _user
    {
        get { return (BO.User)GetValue(_userProperty); }
        set { SetValue(_userProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _user.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _userProperty =
        DependencyProperty.Register("_user", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));

    //private void TextBox_TextChange(object sender, RoutedEventArgs e)
    //{
    //    var textBox = sender as TextBox;    
    //    if(textBox != null)
    //        _user = _s_bl.User.Read(textBox.Text);
    //}
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        if (passwordBox != null)
        {
            if (_user.password != passwordBox.Password)
            {
                MessageBox.Show("Wrong password", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _user.password = passwordBox.Password;
            }

            //    }

            // _user= _s_bl.User.Read(_user.userName);
            //    string password = passwordBox.Password;
            //    if (_user.password != password)
            //    { 
            //        MessageBox.Show("Wrong password", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //    else
            //    {
            //        _user.password = password;


            //    }
            //}
        } }

        private void Click_LogIn(object sender, RoutedEventArgs e)
    {
        try
        {
            Button button = (Button)sender;
            Grid parentGrid = (Grid)button.Parent;

            TextBox usernameTextBox = (TextBox)parentGrid.Children[1];
            PasswordBox passwordBox = (PasswordBox)parentGrid.Children[3];

            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
           // _s_bl.User.Read(_user.userName);
            _user = _s_bl.User.Read(username);


            if (_user.password != password)
                MessageBox.Show("Wrong password", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
            else

                _user.password = password;
            new MainWindow(_user).Show();
            //  this.Close();

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        

    }

    private void Button_Click_SignUp(object sender, RoutedEventArgs e)
    {
        new SignUpWindow().ShowDialog();

    }

}
