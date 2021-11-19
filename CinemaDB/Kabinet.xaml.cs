﻿using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CinemaDB
{
    /// <summary>
    /// Логика взаимодействия для Kabinet.xaml
    /// </summary>
    public partial class Kabinet : Page
    {
        public Kabinet()
        {
            InitializeComponent();
        }
        Пользователи tek;
        public Kabinet(Пользователи tek)
        {
            InitializeComponent();
            this.tek = tek;
            obnovInfo();
        }

        private void obnovInfo()
        {
            TBName.Text = "Имя: " + tek.Имя;
            TBFam.Text = "Фамилия: " + tek.Фамилия;
            TBPol.Text = "Пол: " + tek.Пол1.Пол1;
            TBRol.Text = "Роль: " + tek.Роли.Роль;
            if (tek.Фото != null && tek.Фото.Изображение != null)
            {
                BitmapImage BImage = new BitmapImage();
                using (MemoryStream MS = new MemoryStream(tek.Фото.Изображение))
                {
                    BImage.BeginInit();
                    BImage.StreamSource = MS;
                    BImage.CacheOption = BitmapCacheOption.OnLoad;
                    BImage.EndInit();
                }
                Photo.Source = BImage;
            }
        }
        private void BtnRedactClick(object sender, RoutedEventArgs e)
        {
            RedactProfil p = new RedactProfil(tek);
            p.ShowDialog();
            obnovInfo();
        }

        private void BtnRedactPhotoClick(object sender, RoutedEventArgs e)
        {
            Фото photo = tek.Фото;
            OpenFileDialog dial = new OpenFileDialog();
            dial.ShowDialog();
            string path = dial.FileName;
            System.Drawing.Image SDI = System.Drawing.Image.FromFile(path);
            ImageConverter ic = new ImageConverter();
            byte[] bytePhoto = (byte[])ic.ConvertTo(SDI, typeof(byte[]));
            if (photo == null)
            {
                dbcl.dbP.Фото.Add(new Фото() { id = tek.id, Путь = null, Изображение = bytePhoto });
            }
            else
            {
                photo.Изображение = bytePhoto;
            }
            dbcl.dbP.SaveChanges();

            obnovInfo();
        }
    }
}
