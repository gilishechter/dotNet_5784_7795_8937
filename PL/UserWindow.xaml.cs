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
        InitializeComponent();
        //_s_bl.InitializeDB(); //for list
    }

    public string? _userName
    {
        get { return (string?)GetValue(_userNameProperty); }
        set { SetValue(_userNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for _user.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty _userNameProperty =
        DependencyProperty.Register("_userName", typeof(string), typeof(UserWindow), new PropertyMetadata(null));

    string? password = null;

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        password = (sender as PasswordBox)?.Password;      
    }

    private void Click_LogIn(object sender, RoutedEventArgs e)
    {
        try
        {         
            BO.User _user = _s_bl.User.Read(_userName!)!;


            if (_user.password != password)
                MessageBox.Show("Wrong password", "Try again", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                _user.password = password;
                new MainWindow(_user).Show();              
            }

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
