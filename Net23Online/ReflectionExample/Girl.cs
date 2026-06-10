namespace ReflectionExample;

public class Girl
{
    public string Name { get; set; }
    public string MyVibe { get; }

    private int _age;
    private int _money;

    public bool glass;

    public Girl(int age)
    {
        _age = age;
    }

    public bool IsAdult()
    {
        return _age > 18;
    }

    private int HowGoodIAm()
    {
        return 20;  
    }
}
