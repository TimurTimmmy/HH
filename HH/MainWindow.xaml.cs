﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using HH.actions;
using Newtonsoft.Json;

namespace HH
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Глобальные переменные.
        public int pages;
        public int curpage;
        public int found;
        public int vacpp;
        public string url;
        public string EmplUrl { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем формат строки, указываем кол-во вакасний на страницу, кол-во страниц и сам текст запроса.
            url = string.Format(@"https://api.hh.ru/vacancies?per_page={2}&page={1}&text={0}", SearchText.Text, curpage, vacpp);

            // Загрузка результатов.
            LoadResult(Actions.Get_http(url));
            MessageBox.Show("Найдено " + found + " вакансий.", "Результат");
        }        

        public void LoadResult(string jstring)
        {
            // Объявление коллекции вакасний.
            var ItemList = new List<Item>();

            // Парсинг строки в класс Vac.
            try
            {
                var vac = (Vac)JsonConvert.DeserializeObject<Vac>(jstring);
            
                // Количесвто страниц.
                pages = vac.Pages;
            
                // Текущая страница.
                curpage = vac.Page;
            
                // Найдено вакансий.
                found = vac.Found;  
            
                //Заполняем коллекцию найденными вакансиями.
                foreach (var it in vac.Items)
                {
                    ItemList.Add(it);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Повторите поиск с более точными данными /n Пример: Вакансия Город Оклад", "Error");
                Logging.WriteLog(e.Message + "\n" + e.StackTrace + "\n");
            }
       
            // Вывод средней ЗП из найденного списка.            
            AvgSalary.Content = "Средняя ЗП = " + Item.AVGL(ItemList);

            // Загрузка списка в таблицу.
            ResultGrid.SelectionChanged -= ResultGrid_SelectionChanged;
            ResultGrid.ItemsSource = ItemList;
            ResultGrid.SelectionChanged += ResultGrid_SelectionChanged;

            // Отображаение информации о текущей странице.
            LabelPages.Content = string.Format("Страница {0}", curpage);

            //Активация кнопки переключения страниц.
            NextButton.IsEnabled = true;
        }

        private void ResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Очистка форм
            UrlLabel.Text = null;
            TbDescryption.Text = null;
            MetroLabel.Content = null;

            //Заполнение списка из выбранного поля в таблице.
            DataGrid vac = (DataGrid)sender;
            Item VacId = (Item)vac.SelectedValue;

            // Получение ссылки на сайт работадателя.
            EmplUrl =  Actions.GetEmplUrl(VacId.Employer.Id);

            // Вывод в текстовй блок информации
            TbDescryption.Text = "Краткое описание: \n";

            // Если требования в вакансии не пусто, то выводить их.
            // Если пусто, то сообщать об этом.
            if (VacId.Snippet != null)
                {
                TbDescryption.Text += Actions.NotNull(VacId.Snippet.Requirement) + "\n \n";
                TbDescryption.Text += Actions.NotNull(VacId.Snippet.Responsibility) + "\n \n";
                }           
            else { TbDescryption.Text += "Описания нет" + "\n \n"; }

            // Если информаци об адресе не пусто, то выводить город и метро.
            if (VacId.Address != null)
            {
                TbDescryption.Text += "Город: " + Actions.NotNull(VacId.Address.City) + "\n" 
                    + "Улица: " + Actions.NotNull(VacId.Address.Street) + " " + "Дом: " + Actions.NotNull(VacId.Address.Building);
                {
                    // Если информация о метро не пусто, то выводить название станции.
                    // Иначе выводить "Не задано".
                    if (VacId.Address.Metro != null)
                    {
                        MetroLabel.Content = "Метро: " + VacId.Address.Metro.StationName;
                        MetroLabel.Foreground = Metro.GetColor(VacId.Address.Metro);
                    }
                    else
                    {
                        MetroLabel.Content = "Не задано";
                        MetroLabel.Foreground = Brushes.Black;
                    }
                }
            }
            else { TbDescryption.Text += "Информации нет" + "\n"; MetroLabel.Foreground = Brushes.Black; MetroLabel.Content = "Метро: Информации нет"; }

            // Загрузка логотипа компании.
            EmplLogo.Source = LogoUrls.LoadLogo(VacId.Employer.LogoUrls);            

            // Формирование гиперссылки на выбранную вакансию.
            Hyperlink hyperLink = new Hyperlink()
            {
                NavigateUri = new System.Uri(VacId.AlternateUrl.ToString())
            };
            hyperLink.Inlines.Add(VacId.AlternateUrl.ToString());
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            UrlLabel.Inlines.Add(hyperLink);
        }

        // Кнопка перехода на следующую страницу.
        // Переключает страницу на +1 и перезагружает список.
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {            
            if (curpage < pages - 1)
            {
                curpage += 1;
                LabelPages.Content = string.Format("Страница {0}", curpage);
                url = string.Format(@"https://api.hh.ru/vacancies?per_page={2}&page={1}&text={0}", SearchText.Text, curpage, vacpp);
                LoadResult(Actions.Get_http(url));
            }
        }

        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            Actions.ExportToExcel(ResultGrid);
        }

        // Обработчик гиперссылки.
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        // Выбор кол-ва отображаемых вакансий.
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            vacpp = int.Parse((string)pressed.Content);
            curpage = 0;
        }
    }
}