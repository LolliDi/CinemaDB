using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для WindowCaptcha.xaml
    /// </summary>
    public partial class WindowCaptcha : Window
    {
        string _captch = "";
        DispatcherTimer dProgress = new DispatcherTimer(); //для прогресс бара
        public WindowCaptcha()
        {
            InitializeComponent();
            CaptchaOtv.opnPage = true;
            ImagCaptch.Source = BitmapToImageSource(CreateImage(Convert.ToInt32(ImagCaptch.Width), Convert.ToInt32(ImagCaptch.Height)));
            dProgress.Interval = new TimeSpan(0, 0, 1); //хотел сделать сразу на 10 сек, но мне приспичило сделать вывод оставшегося времени, поэтому сделал посекундно
            dProgress.Tick += new EventHandler(dTimerProgress);
            dProgress.Start();

        }
        private void dTimerProgress(object s, EventArgs e) //увеличиваем значение прогресса
        {
            PTime.Value++;
            if (PTime.Value == 10)
            {
                CaptchaOtv.opnPage = false;
                CaptchaOtv.captcha = false;
                this.Close();
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap) //конвертация битмапа в битмапимадж
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TBEnterCaptch.Text == _captch) //если кача верна, то выходим
            {
                CaptchaOtv.captcha = true;
                CaptchaOtv.opnPage = false;
                this.Close();
            }
            else 
            {
                PTime.Value = 0;
                dProgress.Stop();
                MessageBox.Show("Капча введена неверно!");
                ImagCaptch.Source = BitmapToImageSource(CreateImage(Convert.ToInt32(ImagCaptch.Width), Convert.ToInt32(ImagCaptch.Height))); //меняю капчу
                dProgress.Start();

            }
        }
        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 135);
            int Ypos = rnd.Next(15, Height - 45);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((System.Drawing.Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            _captch = String.Empty;
            string ALF = "123456789";
            for (int i = 0; i < 5; ++i)
                _captch += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(_captch,
                         new Font("Arial", 35),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            //Добавим немного помех
            //Линии из углов
            g.DrawLine(Pens.Black,
                       new System.Drawing.Point(0, 0),
                       new System.Drawing.Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new System.Drawing.Point(0, Height - 1),
                       new System.Drawing.Point(Width - 1, 0));
            //Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void TBEnterCaptch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBEnterCaptch.Text == _captch)
            {
                CaptchaOtv.captcha = true;
                CaptchaOtv.opnPage = false;
                this.Close();
            }
        }
    }
}
