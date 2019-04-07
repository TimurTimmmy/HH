namespace HH
{
    public class Employer
    {
        private string name;
        private string city;
        private string metro;
        private string line_id;
        private string street;
        private string building;
        private string logo_urls;

        public string Name { get => name; set => name = value; }
        public string City { get => city; set => city = value; }
        public string Metro { get => metro; set => metro = value; }
        public string Line_id { get => line_id; set => line_id = value; }
        public string Street { get => street; set => street = value; }
        public string Building { get => building; set => building = value; }
        public string Logo_urls { get => logo_urls; set => logo_urls = value; }

        public Employer(string name, string city, string metro, string line_id, string street, string building, string logo_urls)
        {
            this.Name = NotNull(name);
            this.City = NotNull(city);
            this.Metro = NotNull(metro);
            this.Line_id = NotNull(line_id);
            this.Street = NotNull(street);
            this.Building = NotNull(building);
            this.Logo_urls = NotNull(logo_urls);
        }

        private string NotNull(string value)
        {
            if (
                value == null || value == "" || value == string.Empty || value.Length == 0  
               ) return value;
            else return "Не указано";                    
        }     

    }
}
