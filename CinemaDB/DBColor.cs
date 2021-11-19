using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CinemaDB
{
    public partial class Залы
    {
        public SolidColorBrush TimeColor
        {
            get
            {
                if (Дата.Hour < 13 && Дата.Hour > 5) return new SolidColorBrush(Color.FromArgb(50, 252, 221, 126));
                else if (Дата.Hour > 12 && Дата.Hour < 19) return new SolidColorBrush(Color.FromArgb(50, 126, 252, 245));
                else return new SolidColorBrush(Color.FromArgb(50, 0, 47, 85));
            }
        }

        public string Prodano
        {
            get
            {
                return "Продано: " + Продано;
            }
        }

        public string Cena
        {
            get
            {
                return "Цена: " + Цена;
            }
        }
    }
}
