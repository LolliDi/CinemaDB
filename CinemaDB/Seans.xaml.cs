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
    /// Логика взаимодействия для Seans.xaml
    /// </summary>
    public partial class Seans : Page
    {
        public Seans()
        {
            InitializeComponent();
            LVFilm.ItemsSource = dbcl.dbP.Фильмы.ToList();
            
        }

        private void InfZalLoaded(object sender, RoutedEventArgs e)
        {
            ListView LV = (ListView)sender;
            int ind = Convert.ToInt32(LV.Uid);
            List<Сеансы> inf = dbcl.dbP.Сеансы.Where(x => x.Фильм == ind).ToList();

            LV.ItemsSource = inf;
        }

        private void InfSeansLoaded(object sender, RoutedEventArgs e)
        {
            ListView LV = (ListView)sender;
            int ind = Convert.ToInt32(LV.Uid);
            List<Залы> inf = dbcl.dbP.Залы.Where(x => x.Сеанс == ind).ToList();
            LV.ItemsSource = inf;
        }

        private void TBInfZal(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int ind = Convert.ToInt32(tb.Uid);
            tb.Text = dbcl.dbP.Информация_о_залах.FirstOrDefault(x => x.id == ind).Особенности;
        }

        private void TBKolZal(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int ind = Convert.ToInt32(tb.Uid);
            tb.Text = dbcl.dbP.Информация_о_залах.FirstOrDefault(x => x.id == ind).Количество_мест.ToString();
        }

        private void OstalosLoaded(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            int uid = Convert.ToInt32(tb.Uid);
            int zal = dbcl.dbP.Залы.FirstOrDefault(x => x.id == uid).Зал;
            int prod = dbcl.dbP.Информация_о_залах.FirstOrDefault(x => x.id == zal).Количество_мест - dbcl.dbP.Залы.FirstOrDefault(x => x.id == uid).Продано;
            tb.Text = prod.ToString();
        }

        private void IzmenFilmClick(object sender, RoutedEventArgs e)
        {
            foreach(Window w in Application.Current.Windows)
            {
                if(w.GetType()==typeof(MainWindow))
                {
                    MainWindow s = (MainWindow)w;
                    Button btn = (Button)sender;
                    int uid = Convert.ToInt32(btn.Uid);
                    s.dobavstr(new DobiRedFilm(dbcl.dbP.Фильмы.FirstOrDefault(x => x.id == uid)),s.i+1);
                }
            }
        }

        private void IzmenSeansClick(object sender, RoutedEventArgs e)
        {
            foreach(Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(MainWindow))
                {
                    MainWindow s = (MainWindow)w;
                    Button btn = (Button)sender;
                    int uid = Convert.ToInt32(btn.Uid);
                    s.dobavstr(new DobiRedFilm(dbcl.dbP.Залы.FirstOrDefault(x => x.id == uid)), s.i + 1);
                }
            }
        }

        private void UdalSeansClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int ind = Convert.ToInt32(btn.Uid);
            Залы udzal = dbcl.dbP.Залы.FirstOrDefault(x => x.id == ind);
            int idSeans = udzal.Сеанс;
            dbcl.dbP.Залы.Remove(udzal);
            dbcl.dbP.SaveChanges();
            if(dbcl.dbP.Залы.Where(x=>x.Сеанс==idSeans).ToList().Count<1)
            {
                dbcl.dbP.Сеансы.Remove(dbcl.dbP.Сеансы.FirstOrDefault(X => X.id == idSeans));
                dbcl.dbP.SaveChanges();
            }
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(MainWindow))
                {
                    MainWindow s = (MainWindow)w;
                    s.dobavstr(new Seans(), s.i);
                }
            }
        }

        private void UdalFilmClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int ind = Convert.ToInt32(btn.Uid);
            Фильмы udzal = dbcl.dbP.Фильмы.FirstOrDefault(x => x.id == ind);
            dbcl.dbP.Фильмы.Remove(udzal);
            dbcl.dbP.SaveChanges();
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(MainWindow))
                {
                    MainWindow s = (MainWindow)w;
                    s.dobavstr(new Seans(), s.i);
                }
            }
        }
    }
}
