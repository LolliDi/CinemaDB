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
        public virtual Информация_о_залах Информация_о_залах { get; set; }
        public virtual Сеансы Сеансы { get; set; }
    }
}
