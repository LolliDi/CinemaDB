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

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для AdmMainPage.xaml
    /// </summary>
    public partial class AdmMainPage : Page
    {
        public AdmMainPage()
        {
            InitializeComponent();
        }

        private void InfPolzClick(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows) //поиск окна
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    MainWindow ci = (MainWindow)window;
                    ci.dobavstr(new AdmDannieOPolzPage(), ci.i+1);
                }
            }
        }
    }
}
