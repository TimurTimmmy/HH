using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Newtonsoft.Json.Linq;

namespace HH.actions
{
    public class Actions
    {
        // Получение ссылки запроса.
        public static string Get_http(string url)
        {
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
        
        // Получение ссылки на работадателя
        public static string GetEmplUrl(string id)
        {
            string site_url, jurl;
            jurl = String.Format(@"https://api.hh.ru/employers/{0}", id);
            JObject jObject = JObject.Parse(Actions.Get_http(jurl));
            site_url = (string)jObject["site_url"];
            return site_url;
        }

        // Проверка на пустую строку.
        public static string NotNull(string text)
        {
            if (String.IsNullOrEmpty(text)) { return "Не задано"; } else { return text; }            
        }

        public static void ExportToExcel(DataGrid dgDisplay)
        {
            dgDisplay.SelectAllCells();
            dgDisplay.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dgDisplay);
            string resultat = "Выгрузка от: " + DateTime.Now + "\n"+ (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            StreamWriter file1 = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "export.xls", false, Encoding.UTF8);
            file1.WriteLine(resultat.Replace(',', ' '));
            file1.Close();
        }
    }
}
