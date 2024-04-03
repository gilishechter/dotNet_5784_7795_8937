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

        public string _name { get; set; }
        //{
        //    get { return (string)GetValue(_nameProperty); }
        //    set { SetValue(_nameProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for _name.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty _nameProperty =
        //    DependencyProperty.Register("_name", typeof(string), typeof(SignUpWindow), new PropertyMetadata(null));

        public int _id { get; set; }


        public bool isCreate
        {
            get { return (bool)GetValue(isUpdateProperty); }
            set { SetValue(isUpdateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isUpdate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty isUpdateProperty =
            DependencyProperty.Register("isUpdate", typeof(bool), typeof(SignUpWindow), new PropertyMetadata(true));

        public string? userName { get; set; }
        public string? password { get; set; }

        public SignUpWindow(BO.Worker ?_worker=null)
        {
            if(_worker != null)
            {               
                _id = _worker.Id;
                _name = _worker.Name;
                isCreate = false;
            }
            InitializeComponent();
        }

        private void Button_Click_Cont(object sender, RoutedEventArgs e)
        {          
           
            try
            {               
                BO.Worker worker1 = _s_bl.Worker.Read(_id)!;

                if (worker1!.Name != _name)
                {
                    MessageBox.Show("This worker dosen't exist", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    BO.User user = new() { password = password, userName = userName, isAdmin = false ,Id=_id};
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
