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
            initial();
        }

        private string _cena = "";
        private string _prodano = "";
        private Regex _chislo = new Regex(@"\d*");
        public void initial()
        {
            InitializeComponent();
            CBOgran.ItemsSource = dbcl.dbP.Ограничения.ToList();
            CBZhanr.ItemsSource = dbcl.dbP.Жанры.Select(x => x.Жанр).ToList();
            NazvFilm.ItemsSource = dbcl.dbP.Фильмы.Select(x => x.Название).ToList();
            CBZal.ItemsSource = dbcl.dbP.Информация_о_залах.ToList();
        }
        public DobiRedFilm(string nazv)
        {
            initial();
            NazvFilm.Text = nazv;
        }

        Фильмы newFilm;
        Залы newSeans;
        public DobiRedFilm(Фильмы film) : base() //для редактирования фильма
        {
            initial();
            CBOgran.ItemsSource = dbcl.dbP.Ограничения.ToList();
            CBZhanr.ItemsSource = dbcl.dbP.Жанры.Select(x => x.Жанр).ToList();
            NazvFilm.ItemsSource = dbcl.dbP.Фильмы.Select(x => x.Название).ToList();
            CBZal.ItemsSource = dbcl.dbP.Информация_о_залах.ToList();
            seans = false;
            SPanelDate.Visibility = Visibility.Hidden;
            SPanelProdano.Visibility = Visibility.Hidden;
            SPanelZalCena.Visibility = Visibility.Hidden;
            newFilm = film;
            NazvFilm.Text = newFilm.Название;
            CBZhanr.Text = newFilm.Жанры.Жанр;
            CBOgran.SelectedIndex = dbcl.dbP.Ограничения.FirstOrDefault(x => x.Возраст == newFilm.Ограничения.Возраст).id - 1;
            TBDate.Text = newFilm.Дата_выхода.ToString("HH:mm dd.MM.yyyy");
        }

        public DobiRedFilm(Залы Zal) //для редактирования сеанса
        {
            initial();
            CBOgran.ItemsSource = dbcl.dbP.Ограничения.ToList();
            CBZhanr.ItemsSource = dbcl.dbP.Жанры.Select(x => x.Жанр).ToList();
            NazvFilm.ItemsSource = dbcl.dbP.Фильмы.Select(x => x.Название).ToList();
            CBZal.ItemsSource = dbcl.dbP.Информация_о_залах.ToList();
            newSeans = Zal;
            NazvFilm.Text = dbcl.dbP.Фильмы.FirstOrDefault(x=>x.id==dbcl.dbP.Сеансы.FirstOrDefault(b=>b.id==newSeans.Сеанс).Фильм).Название;
            CBZal.SelectedIndex = newSeans.Зал-1;
            CBZal.IsEnabled = false;
            TBCena.Text = newSeans.Цена.ToString();
            TBDateSeans.Text = newSeans.Дата.ToString("HH:mm dd.MM.yyyy");
            TBProdano.Text = newSeans.Продано.ToString();
            NazvFilm.IsEnabled = false;
        }

        bool film = false; //существует ли фильм
        bool seans = true; //true - добавляем сеанс, false - изменяем фильм
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
            if (seans) //если изменяем фильм, то не блокируем ничего
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
                    CBOgran.SelectedIndex = dbcl.dbP.Ограничения.FirstOrDefault(x=>x.Возраст==p.Ограничения.Возраст).id-1;
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
        }

        private void SohrClick(object sender, RoutedEventArgs e) //сохранение данных
        {
            string rez="";
            try
            {
                MainWindow s = null;
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.GetType() == typeof(MainWindow))
                    {
                        s = (MainWindow)w;
                    }
                }
                string nazv = NazvFilm.Text, ogr = Convert.ToString(CBOgran.SelectedIndex+1), dateVih = TBDate.Text, zhanr = CBZhanr.Text, zal = (CBZal.SelectedIndex+1).ToString(), cena = TBCena.Text, datSeans = TBDateSeans.Text, prodano = TBProdano.Text;
                if ((film || (nazv != "" && ogr != "" && dateVih != "" && zhanr != "")) && (!seans || zal != "" && cena != "" && datSeans != "" && prodano != ""))
                {
                    int indogr = Convert.ToInt32(ogr);
                    ogr = dbcl.dbP.Ограничения.FirstOrDefault(x => x.id == indogr).Возраст;
                    Regex form = new Regex(@"\d\d?:\d\d? \d\d?\.\d\d?\.\d\d\d\d");
                    if (!film&&newSeans == null)
                    {
                        if (!form.IsMatch(dateVih))
                        {
                            throw new Exception("Неверный формат пункта \"Дата выхода фильма\"!\nВведите дату в следующем формате:\n15:00 01.01.2000");
                        }
                        Ограничения ogran = dbcl.dbP.Ограничения.FirstOrDefault(x => x.Возраст == ogr);
                        int idOgran = ogran.id;
                        if (idOgran != 0)
                        {
                            Жанры idZhanr = dbcl.dbP.Жанры.FirstOrDefault(x => x.Жанр == zhanr);
                            if(idZhanr==null)
                            {
                                MessageBoxResult res = MessageBox.Show("Такого жанра фильма не существует в базе!\nЖелаете добавить?\nЕсли выберете \"Нет\", то поменяйте жанр сами!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                if (res == MessageBoxResult.Yes)
                                {
                                    dbcl.dbP.Жанры.Add(new Жанры() { Жанр = zhanr });
                                    dbcl.dbP.SaveChanges();
                                    idZhanr = dbcl.dbP.Жанры.FirstOrDefault(x => x.Жанр == zhanr);
                                    rez += "Жанр; ";
                                }
                                else
                                {
                                    return;
                                }
                            }
                            Фильмы p = dbcl.dbP.Фильмы.FirstOrDefault(x => x.Название == nazv);
                            
                            if (newFilm == null) //если добавляем фильм
                            {
                                //if (p == null)
                                {
                                    dbcl.dbP.Фильмы.Add(new Фильмы() { Название = nazv, Дата_выхода = DateTime.ParseExact(dateVih, "H:m d.M.yyyy", null), Жанр = idZhanr.id, Возрастные_ограничения = idOgran });
                                }
                            }
                            else //если изменяем
                            {
                                newFilm.Название = nazv;
                                newFilm.Дата_выхода = DateTime.ParseExact(dateVih, "H:m d.M.yyyy", null);
                                newFilm.Жанр = idZhanr.id;
                                newFilm.Возрастные_ограничения = idOgran;
                                dbcl.dbP.SaveChanges();
                                s.dobavstr(new Seans(), s.i - 1); //после изменения возвращаемся к таблице
                                return;
                            }
                            dbcl.dbP.SaveChanges();
                            rez += "Фильм; ";
                            film = true;
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
                    if (newFilm == null) //если изменяем/добавляем сеанс
                    {
                        int idZal = Convert.ToInt32(zal);
                        int idFilm = dbcl.dbP.Фильмы.FirstOrDefault(x => x.Название == nazv).id;
                        Сеансы idSeans = dbcl.dbP.Сеансы.FirstOrDefault(x => x.Фильм == idFilm && x.Зал == idZal);
                        if (idSeans == null) //если не существует записи о сеансах в данном зале, то добавим
                        {
                            dbcl.dbP.Сеансы.Add(new Сеансы() { Зал = idZal, Фильм = idFilm });
                            dbcl.dbP.SaveChanges();
                            idSeans = dbcl.dbP.Сеансы.FirstOrDefault(x => x.Фильм == idFilm && x.Зал == idZal);
                        }
                        DateTime dSeans = new DateTime();
                        dSeans = DateTime.ParseExact(datSeans, "H:m d.M.yyyy", null);
                        List<Залы> bdzal = new List<Залы>(); //хранит полный список сеансов в зале
                        List<Залы> prov = new List<Залы>(); //хранит список сеансов, отрыв от которых менее трех часов
                        if (newSeans != null) //если изменяем сеанс, то берем в расчет ид текущего
                        {
                            bdzal = dbcl.dbP.Залы.Where(x => x.Сеанс == newSeans.Сеанс).ToList();
                            prov = bdzal.Where(x => (Math.Abs((x.Дата - dSeans).TotalHours) < 3) && x.id != newSeans.id).ToList();
                        }
                        else //иначе берем все
                        {
                            bdzal = dbcl.dbP.Залы.Where(x => x.Сеанс== idSeans.id).ToList();
                            prov = bdzal.Where(x => Math.Abs((x.Дата - dSeans).TotalHours) < 3).ToList();
                        }
                        if (prov.Count>0) //если есть сеансы, расстояние от которых менее 3х часов, то создаем ошибку и пишем какие это сеансы
                        {
                            string otv = "В пределах трех часов до/после данного сеанса уже идет фильм в выбранном зале. Попробуйте поменять время!\nМешающие сеансы:\n";
                            foreach(Залы z in prov)
                            {
                                otv += "Фильм: " + z.Сеансы.Фильмы.Название + "; Время: " + z.Дата.ToString("HH:mm dd.MM.yyyy")+"\n";
                            }
                            throw new Exception(otv);
                        }

                        int prodanoBiletov = Convert.ToInt32(prodano);
                        int kolMest = dbcl.dbP.Информация_о_залах.First(x => x.id == idZal).Количество_мест;
                        if (kolMest<prodanoBiletov)
                        {
                            throw new Exception("В данном зале только " + kolMest + " мест!\nПоменяйте колличество проданых билетов!");
                        }

                        if (newSeans == null) //если добавляем сеанс
                        {
                            dbcl.dbP.Залы.Add(new Залы() { Сеанс = idSeans.id, Зал = idZal, Цена = Convert.ToInt32(cena), Дата = dSeans, Продано = Convert.ToInt32(prodano) });
                        }
                        else //если изменяем
                        {
                            newSeans.Сеанс = idSeans.id;
                            newSeans.Зал = idZal;
                            newSeans.Цена = Convert.ToInt32(cena);
                            newSeans.Дата = dSeans;
                            newSeans.Продано = Convert.ToInt32(prodano);
                        }
                        dbcl.dbP.SaveChanges();
                        rez += "Сеанс - успешно добавлено/изменено!\n";
                        MessageBoxResult res = MessageBox.Show(rez + "Хотите перейти в базу?\nДа - перейти\nНет - отчистить поля\nОтмена - оставить всё как есть", "Выполнено!", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                        switch(res)
                        {
                            case MessageBoxResult.Yes:
                                s.dobavstr(new Seans(), s.i-1);
                                break;
                            case MessageBoxResult.No:
                                s.dobavstr(new DobiRedFilm(), s.i);
                                s.stranpereh[s.i - 1] = new Seans();
                                break;
                            default:
                                s.dobavstr(new DobiRedFilm(nazv), s.i);
                                break;
                        }
                    }
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

        private void changedChislo(TextBox tb, ref string s) //проверка на то, вводим ли мы цифру
        {
            string text = tb.Text;
            if (_chislo.IsMatch(text)) //проверяем цифра ли
            {
                s = text; 
            }
            else
            {
                tb.Text = s;
                tb.SelectionStart = s.Length; // установка курсора в конец
                MessageBox.Show("Сюда можно вводить только цифру!");
            }
            
        }

        private void TBCenaTextChanged(object sender, TextChangedEventArgs e)
        {
            changedChislo(TBCena, ref _cena);
        }

        private void TBProdanoTextChanged(object sender, TextChangedEventArgs e)
        {
            changedChislo(TBProdano, ref _prodano);
        }
    }
}
