namespace FirstConsoleApp.Users
{
    public class Woman : User
    {
        public int Size { get; set; }

        public Woman(string name) : base(name)
        {

        }

        public override void SayMyName()
        {
            Console.WriteLine($"It's a secret");
        }

        public override int GetMyAge()
        {
            return 18;
        }
    }
}
