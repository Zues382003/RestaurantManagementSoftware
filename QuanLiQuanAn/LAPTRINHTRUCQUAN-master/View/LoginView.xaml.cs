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


namespace QUANLICAPHE.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
              if (e.LeftButton == MouseButtonState.Pressed)
              DragMove();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}




        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MinimizeBTN_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoginBTN_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void MinimizeBTN_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
