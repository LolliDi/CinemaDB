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
        bool film = false; //существует ли фильм
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
            string rez="";
            try
            {
                string nazv = NazvFilm.Text, ogr = CBOgran.Text, dateVih = TBDate.Text, zhanr = CBZhanr.Text, zal = CBZal.Text, cena = TBCena.Text, datSeans = TBDateSeans.Text, prodano = TBProdano.Text;
                if ((film || (nazv != "" && ogr != "" && dateVih != "" && zhanr != "")) && zal != "" && cena != "" && datSeans != "" && prodano != "")
                {
                    Regex form = new Regex(@"\d\d?:\d\d? \d\d?\.\d\d?\.\d\d\d\d");
                    if (!film)
                    {
                        if (!form.IsMatch(dateVih))
                        {
                            throw new Exception("Неверный формат пункта \"Дата выхода фильма\"!\nВведите дату в следующем формате:\n15:00 01.01.2000");
                        }
                        int idOgran = dbcl.dbP.Ограничения.FirstOrDefault(x => x.Возраст == ogr).id;
                        if (idOgran != 0)
                        {
                            int idZhanr = dbcl.dbP.Жанры.FirstOrDefault(x => x.Жанр == zhanr).id;
                            if(idZhanr==0)
                            {
                                MessageBoxResult res = MessageBox.Show("Такого жанра фильма не существует в базе!\nЖелаете добавить?\nЕсли выберете \"Нет\", то поменяйте жанр сами!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                if (res == MessageBoxResult.Yes)
                                {
                                    dbcl.dbP.Жанры.Add(new Жанры() { Жанр = zhanr });
                                    dbcl.dbP.SaveChanges();
                                    idZhanr = dbcl.dbP.Жанры.FirstOrDefault(x => x.Жанр == zhanr).id;
                                    rez += "Жанр; ";
                                }
                                else
                                {
                                    return;
                                }
                            }
                            dbcl.dbP.Фильмы.Add(new Фильмы() { Название = nazv, Дата_выхода = DateTime.ParseExact(dateVih, "hh:mm dd.MM.yyyy", null), Жанр = idZhanr, Возрастные_ограничения = idOgran });
                            dbcl.dbP.SaveChanges();
                            rez += "Фильм; ";

                        }
                        else
                        {
                            throw new Exception("Проблема с ограничением возраста!\nИзмените свой выбор или перевыберите текущий вариант.");
                        }
                    }
                    if (!form.IsMatch(datSeans))
                    {
                        throw new Exception("Неверный формат пункта \"Дата сеанса\"!\nВведите дату в следующем формате:\n15:00 01.01.2000");
                    }
                    int idZal = Convert.ToInt32(zal);
                    int idFilm = dbcl.dbP.Фильмы.FirstOrDefault(x => x.Название == nazv).id;
                    int idSeans = dbcl.dbP.Сеансы.FirstOrDefault(x => x.Фильм == idFilm && x.Зал == idZal).id;
                    if (idSeans==0)
                    {
                        dbcl.dbP.Сеансы.Add(new Сеансы() { Зал = idZal, Фильм = idFilm });
                        dbcl.dbP.SaveChanges();
                        idSeans = dbcl.dbP.Сеансы.FirstOrDefault(x => x.Фильм == idFilm && x.Зал == idZal).id;
                    }
                    dbcl.dbP.Залы.Add(new Залы() { Сеанс = idSeans, Зал = idZal, Цена = Convert.ToInt32(cena), Дата = DateTime.ParseExact(datSeans, "hh:mm dd.MM.yyyy", null), Продано = Convert.ToInt32(prodano) });
                    dbcl.dbP.SaveChanges();
                    rez += "Сеанс - успешно добавлено!";
                    MessageBox.Show(rez, "Успешно!");
                }
                else
                {
                    throw new Exception("Введены не все данные!\nПерепроверьте данные и введите недостающие.");
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(""+ee, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
