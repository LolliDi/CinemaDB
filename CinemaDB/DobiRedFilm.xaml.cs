using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для DobiRedFilm.xaml
    /// </summary>
    public partial class DobiRedFilm : Page
    {
        public DobiRedFilm()
        {
            InitializeComponent();
            CBOgran.ItemsSource = dbcl.dbP.Ограничения.Select(x => x.Возраст).ToList();
            CBZhanr.ItemsSource = dbcl.dbP.Жанры.Select(x => x.Жанр).ToList();
            NazvFilm.ItemsSource = dbcl.dbP.Фильмы.Select(x => x.Название).ToList();
            NazvFilm.Text = "";
        }
        bool film = false;
        private void DPDate_Changed(object sender, TextChangedEventArgs e)
        {
            Regex form = new Regex(@"\d\d:\d\d \d\d?\.\d\d?\.\d\d\d\d");
            string text = TBDate.Text;
            if(text==""||!form.IsMatch(text)) //сохраняем ранее введенное время, если формат ввода верный
                text = "00:00 ";
            else
                text=TBDate.Text.Substring(0,6);
            TBDate.Text = text + DPDate.Text;
        }

        private void TBCBChanged(object sender, TextChangedEventArgs e)
        {
            Фильмы p = dbcl.dbP.Фильмы.FirstOrDefault(x => x.Название == NazvFilm.Text);
            if (p != null)
            {
                TBDate.IsEnabled = false;
                DPDate.IsEnabled = false;
                CBZhanr.IsEnabled = false;
                CBOgran.IsEnabled = false;
                TBDate.Text = p.Дата_выхода.ToString("HH:mm dd.MM.yyyy");
                CBZhanr.Text = p.Жанры.Жанр;
                CBOgran.Text = p.Ограничения.Возраст;
                film = true;
            }
            else
            {
                TBDate.IsEnabled = true;
                DPDate.IsEnabled = true;
                CBZhanr.IsEnabled = true;
                CBOgran.IsEnabled = true;
                film = false;
            }
        }

        private void SohrClick(object sender, RoutedEventArgs e)
        {
            
            string nazv = NazvFilm.Text, ogr = CBOgran.Text, dateVih = TBDate.Text, zhanr = CBZhanr.Text, zal = CBZal.Text, cena = TBCena.Text, datSeans = TBDateSeans.Text, prodano = TBProdano.Text;
            if ((film || (nazv != "" && ogr != "" && dateVih != "" && zhanr != "")) && zal!=""&&cena!=""&&datSeans!=""&&prodano!="")
            {
                Regex form = new Regex(@"\d\d:\d\d \d\d?\.\d\d?\.\d\d\d\d");
                if (!film)
                {
                    if(!form.IsMatch(dateVih))
                    {
                        MessageBox.Show("Введены не все данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                

            }
            else
            {
                MessageBox.Show("Введены не все данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void DPDateSeansChanged(object sender, TextChangedEventArgs e)
        {
            Regex form = new Regex(@"\d\d:\d\d \d\d?\.\d\d?\.\d\d\d\d");
            string text = TBDateSeans.Text;
            if (text == "" || !form.IsMatch(text)) //сохраняем ранее введенное время, если формат ввода верный
                text = "00:00 ";
            else
                text = TBDateSeans.Text.Substring(0, 6);
            TBDateSeans.Text = text + DPDateSeans.Text;
        }
    }
}
