using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для RedactProfil.xaml
    /// </summary>
    public partial class RedactProfil : Window
    {
        Пользователи tek;
        public RedactProfil(Пользователи tek)
        {
            InitializeComponent();
            this.tek = tek;
        }

        private void BtnRedactPasClick(object sender, RoutedEventArgs e)
        {
            BtnProf.Visibility = Visibility.Visible;
            BtnPar.Visibility = Visibility.Collapsed;
            BtnLog.Visibility = Visibility.Visible;
            SPLog.Visibility = Visibility.Collapsed;
            SPParol.Visibility = Visibility.Visible;
            SPPolz.Visibility = Visibility.Collapsed;
        }

        private void BtnRedactLogClick(object sender, RoutedEventArgs e)
        {
            BtnProf.Visibility = Visibility.Visible;
            BtnPar.Visibility = Visibility.Visible;
            BtnLog.Visibility = Visibility.Collapsed;
            SPLog.Visibility = Visibility.Visible;
            SPParol.Visibility = Visibility.Collapsed;
            SPPolz.Visibility = Visibility.Collapsed;
        }
        private void BtnRedactProfClick(object sender, RoutedEventArgs e)
        {
            BtnProf.Visibility = Visibility.Collapsed;
            BtnPar.Visibility = Visibility.Visible;
            BtnLog.Visibility = Visibility.Visible;
            SPLog.Visibility = Visibility.Collapsed;
            SPParol.Visibility = Visibility.Collapsed;
            SPPolz.Visibility = Visibility.Visible;
        }
        private void BtnSohrClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SPPolz.Visibility == Visibility.Visible) //если изменяем данные
                {
                    string otv = "";
                    if (TBName.Text != "")
                    {
                        tek.Имя = TBName.Text;
                        otv = "Имя, ";
                    }
                    if (TBFam.Text != "")
                    {
                        tek.Фамилия = TBFam.Text;
                        otv += "Фамилия, ";
                    }
                    if (otv.Length != 0)
                    {
                        otv.Substring(0, otv.Length - 2);
                        otv += "\nСохранено!";
                        dbcl.dbP.SaveChanges();
                    }
                    else
                    {
                        otv = "Изменений нет!";
                    }
                    MessageBox.Show(otv);
                    Close();
                    return;
                }
                if (SPParol.Visibility == Visibility.Visible) //если меняем пароль
                {

                    string pas = TekParol.Password;
                    string newpas = NewParol.Password;
                    string povtor = NewParolPovt.Password;
                    if (pas != "" && newpas != "" && povtor != "")
                    {
                        if (pas.GetHashCode().ToString() == tek.Пароль)
                        {
                            string strParol = "";
                            {
                                Regex ZaglB = new Regex(@"[A-Z]"); //условия для пароля
                                Regex StrB = new Regex(@"[a-z]");
                                Regex Cifr = new Regex(@"[0-9]");
                                Regex Spec = new Regex(@"\W");
                                if (ZaglB.Matches(newpas).Count < 1) strParol += "Должна быть минимум одна заглавная латинская буква\n"; //проверяем подходит ли пароль под условия
                                if (StrB.Matches(newpas).Count < 3) strParol += "Должно быть минимум три строчных латинских букв\n";
                                if (Cifr.Matches(newpas).Count < 2) strParol += "Должно быть не менее двух цифр\n";
                                if (Spec.Matches(newpas).Count < 1) strParol += "Должно быть не менее одного спецсимвола\n";
                                if (newpas.Length < 8) strParol += "Длинна пароля должна быть не менее восьми символов\n";
                                if (strParol == "")
                                {
                                    if (newpas == povtor)
                                    {
                                        tek.Пароль = newpas.GetHashCode().ToString();
                                        dbcl.dbP.SaveChanges();
                                        MessageBox.Show("Пароль сохранен!");
                                        Close();
                                        return;
                                    }
                                    else
                                    {
                                        throw new Exception("Новые пароли не совпадают!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Пароль не подходит по следующим параметрам:\n" + strParol);
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Старый пароль введен неверно!");
                        }
                    }
                    {
                        throw new Exception("Введены не все данные!");
                    }

                }
                if (SPLog.Visibility == Visibility.Visible) //если изменяем логин
                {
                    string log = TBLog.Text;
                    string par = PBParol.Password;
                    if (log != "" && par != "")
                    {
                        if (par.GetHashCode().ToString() == tek.Пароль)
                        {
                            tek.Логин = log;
                            dbcl.dbP.SaveChanges();
                            MessageBox.Show("Логин сохранен!");
                            Close();
                            return;
                        }
                        else
                        {
                            throw new Exception("Пароль введен неверно!");
                        }
                    }
                    else
                    {
                        throw new Exception("Введены не все данные!");
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }
        }
    }
}
