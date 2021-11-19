using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int i = 0; //страница сейчас
        int ii = 0; //страниц всего
        public List<object> stranpereh = new List<object>();
        //List<People> polzov = new List<People>();

        public MainWindow()
        {
            InitializeComponent();
            dbcl.dbP = new dbEntities();
            ClFrame.Fr = FrameStr;
            dobavstr(new VhodPage(), i);
            Thread time = new Thread(STime) { IsBackground = true }; //создание потока
            time.Start(); //запуск

        }

        private void STime() //поток с часами
        {
            DateTime d;
            while (true)
            {
                using (var client = new HttpClient()) //используем запросы в интернетике
                {
                    try
                    {
                        var result = client.GetAsync("https://time100.ru/",
                              HttpCompletionOption.ResponseHeadersRead).Result; //получение заголовков сайта
                        d = result.Headers.Date.Value.LocalDateTime; //вытягиваем из заголовка дату для нашего региона
                    }
                    catch //если произошла ошибка или пропало соединение - будем получать время с компа
                    {
                        d = DateTime.Now;
                    }
                    Dispatcher.Invoke(() => { TBTime.Text = d.ToString("HH:mm:ss\ndd.MM.yyyy"); }); //отправляем в очередь диспетчера обновления приложения изменение вывода времени
                    Thread.Sleep(1000);
                }
            }
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e) //перемещение окна мышкой
        {
            DragMove();
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void pereh(int ind) //переход по страницам
        {
            if (ind >= 0 && ind < ii) //если страницы существуют то переходим и меняем номер текущей страницы
            {
                ClFrame.Fr.Navigate(stranpereh[ind]);
                i = ind;
            }
            prover();
        }

        private void prover()
        {
            BNaz.IsEnabled = true;
            BVper.IsEnabled = true;
            if (i <= 0 || i >= ii - 1)
            {
                if (i <= 0)
                {
                    BNaz.IsEnabled = false;
                }
                if (i >= ii - 1)
                {
                    BVper.IsEnabled = false;
                }
            }
        }

        public void dobavstr(object ss, int ind) //добавление страниц, индексом передавать место, в которое вставляем страницу
        {
            if (ind < ii - 1) //если после вставленной страницы есть ещё - их удаляем
            {
                int iii = ind;
                while (iii < ii)
                {
                    stranpereh[iii] = null;
                    stranpereh.RemoveAt(iii);
                    ii--;
                }
            }
            stranpereh.Insert(ind, ss); //вставляем по индексу
            ClFrame.Fr.Navigate(stranpereh[ind]); //переходим
            i = ind;
            ii = ind + 1;
            prover();
        }

        private void vperClick(object sender, RoutedEventArgs e)
        {
            pereh(i + 1);
        }

        private void nazClick(object sender, RoutedEventArgs e)
        {
            pereh(i - 1);
        }

        private void RegistrClick(object sender, RoutedEventArgs e)
        {

            RegPage f = new RegPage();
            if (stranpereh[i].GetType() == f.GetType())
            {
                f = (RegPage)stranpereh[i];
                string name = f.RegName.Text, fam = f.RegFam.Text, login = f.RegLogin.Text, pas = f.RegParol.Password, pol = f.RegPol.Text, rol = f.RegRole.Text;
                if (name == "" || fam == "" || login == "" || pol == "" || rol == "" || pas == "")
                {
                    MessageBox.Show("Введены не все данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Пользователи reg = dbcl.dbP.Пользователи.FirstOrDefault(x => x.Логин == login);
                if (reg == null)
                {
                    string strParol = "";
                    Regex ZaglB = new Regex(@"[A-Z]"); //условия для пароля
                    Regex StrB = new Regex(@"[a-z]");
                    Regex Cifr = new Regex(@"[0-9]");
                    Regex Spec = new Regex(@"\W");
                    if (ZaglB.Matches(pas).Count < 1) strParol += "Должна быть минимум одна заглавная латинская буква\n"; //проверяем подходит ли пароль под условия
                    if (StrB.Matches(pas).Count < 3) strParol += "Должно быть минимум три строчных латинских букв\n";
                    if (Cifr.Matches(pas).Count < 2) strParol += "Должно быть не менее двух цифр\n";
                    if (Spec.Matches(pas).Count < 1) strParol += "Должно быть не менее одного спецсимвола\n";
                    if (pas.Length < 8) strParol += "Длинна пароля должна быть не менее восьми символов\n";
                    if (strParol == "")
                        if (pas == f.RegParolPovt.Password)
                        {
                            Пол p = dbcl.dbP.Пол.FirstOrDefault(x => x.Пол1 == pol);
                            Роли r = dbcl.dbP.Роли.FirstOrDefault(x => x.Роль == rol);
                            if (p == null) //если такого гендера нет, то добавим его :)
                            {
                                dbcl.dbP.Пол.Add(new Пол() { Пол1 = pol });
                                dbcl.dbP.SaveChanges(); //закинули в таблицу и сохранили
                                p = dbcl.dbP.Пол.FirstOrDefault(x => x.Пол1 == pol); //ищем его сущность для получения ида
                                f.RegPol.ItemsSource = dbcl.dbP.Пол.ToList();//обновляем комбобокс с гендерами
                            }
                            dbcl.dbP.Пользователи.Add(new Пользователи() { Имя = name, Фамилия = fam, Пол = p.id, Роль = r.id, Логин = login, Пароль = pas.GetHashCode().ToString() });
                            dbcl.dbP.SaveChanges();
                            dialog("Аккаунт зарегестрирован!\nЖелаете войти?", "Информация", new VhodPage(), i + 1);
                            f.RegFam.Text = "";
                            f.RegLogin.Text = "";
                            f.RegName.Text = "";
                            f.RegParol.Password = "";
                            f.RegPol.Text = "";
                            f.RegRole.Text = "";
                            f.RegParolPovt.Password = "";
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    else
                    {
                        MessageBox.Show("Пароль не подходит по следующим параметрам:\n" + strParol, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    dialog("Пользователь с таким логином уже существует.\nЕсли это ваш аккаунт, то нажмите \"Да\" для входа!", "Ошибка", new VhodPage(), i + 1);
                    return;
                }
            }
            else
            {
                dobavstr(f, i + 1);
            }
        }

        private void VhodClick(object sender, RoutedEventArgs e)
        {
            VhodPage f = new VhodPage();
            if (stranpereh[i].GetType() == f.GetType())//если текущая страница - вход
            {
                f = (VhodPage)stranpereh[i];
                string log = f.VhodLogin.Text, pas = f.VhodPass.Password;
                if (log != "" && pas != "")
                {
                    Пользователи ne = dbcl.dbP.Пользователи.FirstOrDefault(x => x.Логин == log);
                    if (ne != null)
                    {
                        if (ne.Пароль == pas.GetHashCode().ToString()) //проверяем пароль
                        {
                            BtnVhod.IsEnabled = false;
                            BtnReg.IsEnabled = false;
                            BtnVihod.IsEnabled = true;
                            if (ne.Роль == 1)
                                dobavstr(new AdmMainPage(ne), 0);
                            else
                                dobavstr(new Kabinet(ne), 0);
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        dialog("Такого логина не существует!\nЖелаете зарегестрироваться?", "Ошибка авторизации!", new RegPage(), i + 1);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Вы ввели не все данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

            }
            else
            {
                dobavstr(f, i + 1);
            }
            prover();
        }

        private void dialog(string s1, string s2, object ss, int Nstr) //вывод диалога и дальнейший переход на страницу ss, если нужно
        {
            MessageBoxResult r = MessageBox.Show(s1, s2, MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (r)
            {
                case MessageBoxResult.Yes:
                    dobavstr(ss, Nstr);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void HideClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void VihodClick(object sender, RoutedEventArgs e)
        {
            dialog("Вы уверены, что хотите выйти из аккаунта?", "", new VhodPage(), 0);
            if (ii <= 1)
            {
                BtnVhod.IsEnabled = true;
                BtnReg.IsEnabled = true;
                BtnVihod.IsEnabled = false;
            }
        }
    }

}
