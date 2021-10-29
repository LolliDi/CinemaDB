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
    /// Логика взаимодействия для AdmDannieOPolzPage.xaml
    /// </summary>
    public partial class AdmDannieOPolzPage : Page
    {
        public AdmDannieOPolzPage()
        {
            InitializeComponent();
            var result = from Пользователи in dbcl.dbP.Пользователи //объединяем таблицы, чтобы заменить иды пола и роли на названия
                         join Пол in dbcl.dbP.Пол on Пользователи.Пол equals Пол.id
                         join Роли in dbcl.dbP.Роли on Пользователи.Роль equals Роли.id
                         select new
                         {
                             id = Пользователи.id,
                             Фамилия = Пользователи.Фамилия,
                             Имя = Пользователи.Имя,
                             Пол = Пол.Пол1,
                             Роль = Роли.Роль,
                             Логин = Пользователи.Логин,
                             Пароль = Пользователи.Пароль
                         };
            DGDanPolz.ItemsSource = result.ToList();
        }
    }
}
