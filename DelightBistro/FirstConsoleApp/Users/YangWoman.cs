namespace FirstConsoleApp.Users
{
    public class YangWoman : Woman
    {
        public YangWoman(string name) : base(name)
        {

        }

        public override void SayMyName()
        {
            Console.WriteLine($"I'm to yang to name");
        }
    }
}
