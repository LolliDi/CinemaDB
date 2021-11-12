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
        }

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
    }
}
