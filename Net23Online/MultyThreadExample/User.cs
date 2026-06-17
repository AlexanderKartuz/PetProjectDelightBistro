namespace MultyThreadExample;

internal class User
{
    private static object lockObj = new();

    public void Do(Payload payload)
    {
        // wait 1 2 3 4 5 6
        lock (lockObj)
        {
            while (true)
            {
                payload.counter++;
                if (payload.counter % 2 == 0)
                {
                    var isStellGood = payload.counter % 2 == 0;
                    Console.WriteLine(isStellGood
                        ? "+"
                        : "**********************");
                }// sleep 99
            }
        }
    }


    public async Task<string> DoAsync()
    {
        var a = 1;
        a++;
        await GetFromDb(); 
        // ------------------------ //
        a = a * 22;
        Console.WriteLine("End");

        return "Smile";
    }

    public async Task GetFromDb()
    {
    }
}


internal class Payload
{
    public int counter;
}