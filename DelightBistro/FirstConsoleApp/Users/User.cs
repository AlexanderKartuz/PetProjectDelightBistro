namespace FirstConsoleApp.Users
{
    public class User
    {
        // public string name;// field Поле

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == "Ivan")
                {
                    return;
                }
                _name = value;
            }
        }

        public DateTime Birthday { get; set; } // Setting Свойство

        public User(string name)
        {
            Name = name;
            Birthday = DateTime.Now;
        }

        public User(string name, DateTime birthday)
        {
            Name = name;
            Birthday = birthday;
        }

        public virtual void SayMyName()
        {
            Console.WriteLine($"Hi, my name is {Name}");
        }

        public void SayHiForYouFriend(User friend, int age)
        {
            age = -100;
            friend.Name = "Corrupted";
        }

        public virtual int GetMyAge()
        {
            return DateTime.Now.Year - Birthday.Year;
        }
    }
}
