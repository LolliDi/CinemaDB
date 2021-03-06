//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CinemaDB
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;

    public partial class Залы
    {
        public int id { get; set; }
        public int Сеанс { get; set; }
        public int Зал { get; set; }
        public int Цена { get; set; }
        public System.DateTime Дата { get; set; }
        public int Продано { get; set; }

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
        public virtual Информация_о_залах Информация_о_залах { get; set; }
        public virtual Сеансы Сеансы { get; set; }
    }
}
