using System;
using System.Windows.Media.Imaging;
using HH.actions;
using Newtonsoft.Json;

namespace HH
{
    public partial class LogoUrls
    {
        [JsonProperty("90")]
        public string The90 { get; set; }

        [JsonProperty("240")]
        public string The240 { get; set; }

        [JsonProperty("original")]
        public string Original { get; set; }

        public static BitmapImage LoadLogo(LogoUrls logo)
        {
            string LogoPath;
            try
            {
                if (!string.IsNullOrEmpty(logo.The90)) LogoPath = logo.The90;
                else
                    if (!string.IsNullOrEmpty(logo.The240)) LogoPath = logo.The240;
                else
                    if (!string.IsNullOrEmpty(logo.Original)) LogoPath = logo.Original;
                else
                    LogoPath = @"https://pp.userapi.com/c840720/v840720243/855bb/HBaxCm4hv3s.jpg";
            }
            catch (Exception e)
            {                
                Logging.WriteLog(e.Message + "\n" + e.StackTrace + "\n");                
                LogoPath = @"https://pp.userapi.com/c840720/v840720243/855bb/HBaxCm4hv3s.jpg";
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(LogoPath, UriKind.Absolute);
            bitmap.EndInit();
            return bitmap;
        }
    }
}


