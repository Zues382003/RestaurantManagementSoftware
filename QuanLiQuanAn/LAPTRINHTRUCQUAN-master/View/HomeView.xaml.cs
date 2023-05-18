using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QUANLICAPHE.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : System.Windows.Controls.UserControl
    {
        public Func<double, string> formatFunc = (x) => x.ToString() + " VND";

        public HomeView()
        {
            InitializeComponent();
            this.CartesianChart.AxisY.Add(new LiveCharts.Wpf.Axis { LabelFormatter = formatFunc });
            this.PieChart.AxisY.Add(new LiveCharts.Wpf.Axis { LabelFormatter = formatFunc });

        }

        private void ButtonMoveForward_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonMoveBackward_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dvg_TopTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txt_TopTableMonth_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
