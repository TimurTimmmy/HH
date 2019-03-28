
namespace HH
{
    public class Vacancy
    {
        private string id;
        private string name;
        private string description;
        private string minsalary;
        private string maxsalary;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string MinSalary
        {
            get { return minsalary; }
            set { minsalary = value; }
        }
        public string MaxSalary
        {
            get { return maxsalary; }
            set { maxsalary = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Vacancy(string Id, string Name, string MinSalary, string MaxSalary, string Description)
        {
            this.Id = Id;
            this.Name = Name;
            this.MinSalary = MinSalary;
            this.MaxSalary = MaxSalary;
            this.Description = Description;
        }

        public Vacancy(string Id, string Name, string Description)
        {
            this.Id = Id;
            this.Name = Name;
            this.MinSalary = "не указано";
            this.MaxSalary = "не указано";
            this.Description = Description;
        }
    }
}
