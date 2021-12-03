using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для Seans.xaml
    /// </summary>
    public partial class Seans : Page
    {
        private List<Фильмы> _filt = new List<Фильмы>();
        public Seans()
        {
            InitializeComponent();
            CBZhanr.Items.Add("Все");
            foreach (string s in dbcl.dbP.Жанры.Select(x => x.Жанр).ToList())
            {
                CBZhanr.Items.Add(s);
            }
            ChBZ1.IsChecked = true;
            ChBZ2.IsChecked = true;
            ChBZ3.IsChecked = true;
            ChBZ4.IsChecked = true;
            ChBZ5.IsChecked = true;
            ChBZ6.IsChecked = true;
            CBZhanr.SelectedIndex = 0;
            DataContext = pc;
            pc.CountPage = _filt.Count;
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
            foreach (Window w in Application.Current.Windows)
            {
                if (w.GetType() == typeof(MainWindow))
                {
                    MainWindow s = (MainWindow)w;
                    Button btn = (Button)sender;
                    int uid = Convert.ToInt32(btn.Uid);
                    s.dobavstr(new DobiRedFilm(dbcl.dbP.Фильмы.FirstOrDefault(x => x.id == uid)), s.i + 1);
                }
            }
        }

        private void IzmenSeansClick(object sender, RoutedEventArgs e)
        {
            foreach (Window w in Application.Current.Windows)
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
            if (dbcl.dbP.Залы.Where(x => x.Сеанс == idSeans).ToList().Count < 1)
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

        private void filter()
        {
            List<Фильмы> filt1 = new List<Фильмы>();
            int kol=_filt.Count;
            int id = CBZhanr.SelectedIndex;
            if (id == 0)
            {
                _filt = dbcl.dbP.Фильмы.ToList();
            }
            else
            {
                _filt = dbcl.dbP.Фильмы.Where(X => X.Жанр == id).ToList();
            }
            if (TBPoisk.Text != "")
            {
                
                foreach (Фильмы s in _filt)
                {
                    if (s.Название.ToLower().IndexOf(TBPoisk.Text.ToLower()) != -1)
                    {
                        filt1.Add(s);
                    }
                }
                _filt = filt1;
                filt1 = new List<Фильмы>();
            }
            filt1 = ProverChBZ((bool)ChBZ1.IsChecked, 1, filt1);
            filt1 = ProverChBZ((bool)ChBZ2.IsChecked, 2, filt1);
            filt1 = ProverChBZ((bool)ChBZ3.IsChecked, 3, filt1);
            filt1 = ProverChBZ((bool)ChBZ4.IsChecked, 4, filt1);
            filt1 = ProverChBZ((bool)ChBZ5.IsChecked, 5, filt1);
            filt1 = ProverChBZ((bool)ChBZ6.IsChecked, 6, filt1);
            _filt = filt1;
            if (kol != _filt.Count)
            {
                pc.CountPage = pc.CountPage;
                
                pc.Countlist = filt1.Count;
                if (kol != 0)
                {
                    pc.CurrentPage = 1;
                }
            }
            LVFilm.ItemsSource = _filt.Skip(0).Take(pc.CountPage).ToList();

        }

        private List<Фильмы> ProverChBZ(bool check, int zal, List<Фильмы> filt1)
        {
            if (check == true)
            {
                foreach (Фильмы s in _filt)
                {
                    if (s.Сеансы.Where(x => x.Зал == zal).Count() > 0)
                    {
                        if (!filt1.Contains(s))
                        {
                            filt1.Add(s);
                        }
                    }
                }
            }
            return filt1;
        }

        private void CBZhanrSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void TBPoiskTextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void ChBZChecked(object sender, RoutedEventArgs e)
        {
            filter();
        }

        private void VozrastClick(object sender, RoutedEventArgs e)
        {
            if(SortName.IsChecked==true)
            {
                _filt.Sort((x, y) => x.Название.CompareTo(y.Название));
            }
            else if(SortVozr.IsChecked==true)
            {
                _filt.Sort((x, y) => x.Возрастные_ограничения.CompareTo(y.Возрастные_ограничения));
            }
            else
            {
                _filt.Sort((x, y) => x.Дата_выхода.CompareTo(y.Дата_выхода));
            }
            LVFilm.ItemsSource = _filt.Skip(0).Take(pc.CountPage).ToList();
        }

        private void YbivClick(object sender, RoutedEventArgs e)
        {
            VozrastClick(sender, e);
            _filt.Reverse();
            LVFilm.ItemsSource = _filt.Skip(0).Take(pc.CountPage).ToList();
        }

        PageChange pc = new PageChange();

        private void GoPage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)  // определяем, куда конкретно было сделано нажатие
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            LVFilm.ItemsSource = _filt.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();  // оображение записей постранично с определенным количеством на каждой странице
        }

        private void TBKolZapTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(TBKolZap.Text); // если в текстовом поле есnь значение, присваиваем его свойству объекта, которое хранит количество записей на странице
            }
            catch
            {
                pc.CountPage = _filt.Count; // если в текстовом поле значения нет, присваиваем свойству объекта, которое хранит количество записей на странице количество элементов в списке
            }
            pc.Countlist = _filt.Count;  // присваиваем новое значение свойству, которое в объекте отвечает за общее количество записей
            LVFilm.ItemsSource = _filt.Skip(0).Take(pc.CountPage).ToList();  // отображаем первые записи в том количестве, которое равно CountPage
            pc.CurrentPage = 1;
        }
    }
}
