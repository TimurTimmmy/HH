using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using Newtonsoft.Json.Linq;

namespace HH
{
        /// Работадатель
        /// Координаты, метро, подсветка по цвету линии метро
        /// логгирование исключений в файл
        /// исключения
        /// выгрузка в эксель

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public int pages;
        public int curpage;
        public int found;
        public int vacpp;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadResult(Get_http());
            MessageBox.Show("Найдено " + found + " вакансий.", "Результат");
        }
 
        public string Get_http()
        {
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

        public void LoadResult(string jstring)
        {
            var VacList = new List<Vacancy>();
            var VacObj = JObject.Parse(jstring);
            pages = (int)VacObj["pages"];
            curpage = (int)VacObj["page"];
            found = (int)VacObj["found"];
            var items = from jVac in VacObj["items"]
                        where jVac["address"].HasValues where jVac["address"]["metro"].HasValues where jVac["employer"]["logo_urls"].HasValues
                        select new Vacancy
                        (
                        (string)jVac["id"], (string)jVac["name"], (string)jVac["salary"]["from"], (string)jVac["salary"]["to"], 
                        (string)jVac["snippet"]["requirement"]+ "\n" + (string)jVac["snippet"]["responsibility"], (string)jVac["alternate_url"],
                        new Employer((string)jVac["employer"]["name"], (string)jVac["address"]["city"], (string)jVac["address"]["metro"]["station_name"], 
                        (string)jVac["address"]["metro"]["line_id"], (string)jVac["address"]["street"], (string)jVac["address"]["building"], (string)jVac["employer"]["logo_urls"]["original"]) 
                        );
            foreach (var it in items) { VacList.Add(it); }
            AVGL(VacList);
            ResultGrid.SelectionChanged -= ResultGrid_SelectionChanged;
            ResultGrid.ItemsSource = VacList;
            ResultGrid.SelectionChanged += ResultGrid_SelectionChanged;
            LabelPages.Content = string.Format("Страница {0}", curpage);
            NextButton.IsEnabled = true;
        }

        private void ResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UrlLabel.Text = null;
            DataGrid vac = (DataGrid)sender;
            Vacancy VacId = (Vacancy)vac.SelectedValue;
            TbDescryption.Text = "Краткое описание: \n" + VacId.Description + "\n" ;
            Hyperlink hyperLink = new Hyperlink()
            {
                NavigateUri = new System.Uri(VacId.Url)
            };
            hyperLink.Inlines.Add(VacId.Url);
            hyperLink.RequestNavigate += Hyperlink_RequestNavigate;
            UrlLabel.Inlines.Add(hyperLink);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {            
            if (curpage < pages - 1)
            {                
                curpage += 1;
                LabelPages.Content = string.Format("Страница {0}", curpage);
                LoadResult(Get_http());
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            vacpp = int.Parse((string)pressed.Content);
            curpage = 0;
        }

        private void AVGL(List<Vacancy> list)
        {
            int max = 0;
            int min = 0;
            foreach (var it in list)
            {
                 max += Convert.ToInt32(it.MaxSalary) / list.Count;
                 min += Convert.ToInt32(it.MinSalary) / list.Count;
            }
            double result = (max + min) / 2;
            AvgSalary.Content = "Средняя ЗП = " + result.ToString();
        }
    }
}