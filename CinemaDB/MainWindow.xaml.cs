using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String TekPolz = "", Role = ""; //текущий пользователь и роль
        int i = 0;//страница сейчас
        int ii = 0; //страниц всего
        List<object> stranpereh = new List<object>();
        //List<People> polzov = new List<People>();

        public MainWindow()
        {
            InitializeComponent();
            dbcl.dbP = new dbEntities();
            ClFrame.Fr = FrameStr;
            dobavstr(new VhodPage(), i);
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

        private void dobavstr(object ss, int ind) //добавление страниц, индексом передавать место, в которое вставляем страницу
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
                    Regex ZaglB = new Regex(@"[A-Z]");
                    Regex StrB = new Regex(@"[a-z]");
                    Regex Cifr = new Regex(@"[0-9]");
                    Regex Spec = new Regex(@"\W");
                    if (ZaglB.Matches(pas).Count < 1)
                    {
                        strParol += "Должна быть минимум одна заглавная латинская буква\n";
                    }
                    if (StrB.Matches(pas).Count < 3)
                        strParol += "Должно быть минимум три строчных латинских букв\n";
                    if (Cifr.Matches(pas).Count < 2)
                        strParol += "Должно быть не менее двух цифр\n";
                    if (Spec.Matches(pas).Count < 1)
                        strParol += "Должно быть не менее одного спецсимвола\n";
                    if(pas.Length<8)
                        strParol += "Длинна пароля должна быть не менее восьми символов\n";
                    if (strParol == "")
                        if (pas == f.RegParolPovt.Password)
                        {
                            Пол p = dbcl.dbP.Пол.FirstOrDefault(x => x.Пол1 == pol);
                            Роли r = dbcl.dbP.Роли.FirstOrDefault(x => x.Роль == rol);
                            if (p != null && r != null)
                            {
                                Пользователи ne = new Пользователи() { Имя = name, Фамилия = fam, Пол = p.id, Роль = r.id, Логин = login, Пароль = pas.GetHashCode().ToString() };
                                dbcl.dbP.Пользователи.Add(ne);
                                dbcl.dbP.SaveChanges();
                                dialog("Аккаунт зарегестрирован!\nЖелаете войти?", "Информация", new VhodPage());
                            }
                            else
                            {
                                MessageBox.Show("Что-то с полом и ролью", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
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
                    dialog("Пользователь с таким логином уже существует.\nЕсли это ваш аккаунт, то нажмите \"Да\" для входа!", "Ошибка", new VhodPage());
                    return;
                }
            }
            else
            {
                dobavstr(f, i+1);
            }
        }

        private void VhodClick(object sender, RoutedEventArgs e)
        {
            VhodPage f = new VhodPage();
            if (stranpereh[i].GetType() == f.GetType())//если текущая страница - вход
            {
                f = (VhodPage)stranpereh[i];
                string log = f.VhodLogin.Text, pas = f.VhodPass.Password;
                if (log != ""&&pas!="")
                {
                    Пользователи ne = dbcl.dbP.Пользователи.FirstOrDefault(x => x.Логин == log);
                    if (ne != null)
                    {
                        if (ne.Пароль == pas.GetHashCode().ToString()) //проверяем пароль
                        {
                            if (ne.Роль == 1)

                                dobavstr(new AdmMainPage(), 0);
                            else
                                dobavstr(new PolzMainPage(), 0);
                        }
                        else
                        {
                            MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        dialog("Такого логина не существует!\nЖелаете зарегестрироваться?", "Ошибка авторизации!", new RegPage());
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

        private void dialog(string s1, string s2, object ss) //вывод диалога и дальнейший переход на страницу ss, если нужно
        {
            MessageBoxResult r = MessageBox.Show(s1, s2, MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (r)
            {
                case MessageBoxResult.Yes:
                    dobavstr(ss, i + 1);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
    }

}
