using System.Windows;
using System.Windows.Controls;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для AdmMainPage.xaml
    /// </summary>
    public partial class AdmMainPage : Page
    {
        Пользователи tek;
        MainWindow c = null;
        public AdmMainPage(Пользователи tek)
        {
            InitializeComponent();
            this.tek = tek;
            foreach (Window window in Application.Current.Windows) //поиск окна
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    c = (MainWindow)window;
                }
            }
        }

        private void InfPolzClick(object sender, RoutedEventArgs e)
        {
            c.dobavstr(new AdmDannieOPolzPage(), c.i + 1);
        }

        private void InfFilmClick(object sender, RoutedEventArgs e)
        {
            c.dobavstr(new Seans(), c.i + 1);
        }

        private void DobPolzClick(object sender, RoutedEventArgs e)
        {
            c.dobavstr(new DobiRedFilm(), c.i + 1);
        }

        private void KabinetClick(object sender, RoutedEventArgs e)
        {
            c.dobavstr(new Kabinet(tek), c.i + 1);
        }
    }
}
