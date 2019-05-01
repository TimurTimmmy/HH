using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HH
{
        /// Работадатель + лого
        /// логгирование исключений в файл
        /// исключения
        /// выгрузка в эксель
        /// сохранение

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Загрузка результатов.
            LoadResult(Get_http());
            MessageBox.Show("Найдено " + found + " вакансий.", "Результат");            
        }
        
        // Получение ссылки запроса.
        public string Get_http() 
        {
            // Устанавливаем формат строки, указываем кол-во вакасний на страницу, кол-во страниц и сам текст запроса
            string url = string.Format(@"https://api.hh.ru/vacancies?per_page={2}&page={1}&text={0}", SearchText.Text, curpage, vacpp);
            CookieContainer cookies = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "ash123@mail.ru";
            req.CookieContainer = cookies;
            req.Headers.Add("DNT", "1");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string result = sr.ReadToEnd();
            resp.Close();
            sr.Close();
            return result;
        }

        //public string GetEmplUrl()
        //{
        //    string site_url, jurl;
        //    jurl =  @"https://api.hh.ru/employers/39371";

        //    JObject jObject = JObject.
        //    site_url = (string)jObject["site_url"];
        //    return site_url;
        //}

        public void LoadResult(string jstring)
        {
            // Объявление коллекции вакасний.
            var ItemList = new List<Item>();

            // Парсинг строки в класс Vac.
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

            //////////var EmplUrl =  Employer.GetUrl(VacId.Employer.Id);

            // Вывод в текстовй блок информации
            TbDescryption.Text = "Краткое описание: \n";

            // Если требования в вакансии не пусто, то выводить их.
            // Если пусто, то сообщать об этом.
            if (VacId.Snippet != null)
                {
                TbDescryption.Text += NotNull(VacId.Snippet.Requirement) + "\n \n";
                TbDescryption.Text += NotNull(VacId.Snippet.Responsibility) + "\n \n";
                }           
            else { TbDescryption.Text += "Описания нет" + "\n \n"; }

            // Если информаци об адресе не пусто, то выводить город и метро.
            if (VacId.Address != null)
            {
                TbDescryption.Text += "Город: " + NotNull(VacId.Address.City) + "\n" 
                    + "Улица: " + NotNull(VacId.Address.Street) + " " + "Дом: " + NotNull(VacId.Address.Building);

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
                LoadResult(Get_http());
            }
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

        // Проверка на пустую строку.
        public string NotNull(string text)
        {
            if (String.IsNullOrEmpty(text)) { return "Не задано"; } else { return text; }
        }        
    }
}