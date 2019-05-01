using System.Windows.Media;
using Newtonsoft.Json;

namespace HH
{
    public partial class Metro
    {
        [JsonProperty("station_name")]
        public string StationName { get; set; }

        [JsonProperty("line_name")]
        public string LineName { get; set; }

        [JsonProperty("station_id")]
        public string StationId { get; set; }

        [JsonProperty("line_id")]
        public string LineId { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lng")]
        public string Lng { get; set; }

        public static Brush GetColor(Metro metro)
        {            
            switch (metro.LineId)
            {
                case "1":
                    return Brushes.Red;
                case "2":
                    return Brushes.Green;
                case "3":
                    return Brushes.Blue;
                case "4":
                    return Brushes.LightBlue;
                case "5":
                    return Brushes.Brown;
                case "6":
                    return Brushes.Orange;
                case "7":
                    return Brushes.Purple;
                case "8":
                    return Brushes.DarkGoldenrod;
                case "9":
                    return Brushes.DarkGray;
                case "10":
                    return Brushes.LightGreen;
                case "11":
                    return Brushes.LightCyan;
                case "12":
                    return Brushes.DeepSkyBlue;
                case "14":
                    return Brushes.IndianRed;
                default:
                    return Brushes.Black;
            }
        }

    }
}


