namespace FirstConsoleApp
{
    internal class IEnumerableExample
    {
        public void Test()
        {
            // 6 8 10 22 24 25 12 17 10 27 28 12 17 10 30 31
            var list = GenerateNumbers(); // Do not create element

            list = list.Where(x => x % 2 == 0); // Do not create element

            foreach (var item in list)
            {
                if (item > 3)
                {
                    return;
                }

                Console.WriteLine(item);
            }

            IEnumerable<int> GenerateNumbers()
            {
                Console.WriteLine($"method GenerateNumbers START");

                Console.WriteLine($"Generate item 0");
                yield return 0;// pause

                Console.WriteLine($"Generate item 1");
                yield return 1;

                Console.WriteLine($"Generate item 2");
                yield return 2;

                Console.WriteLine($"Generate item 3");
                yield return 3;

                Console.WriteLine($"Generate item 4");
                yield return 4;

                Console.WriteLine($"Generate item 5");
                yield return 5;

                Console.WriteLine($"method GenerateNumbers END");
            }

            //List<int> GenerateNumbers()
            //{
            //    var list = new List<int>();
            //    for (int i = 0; i < 10; i++)
            //    {
            //        list.Add(i);

            //        Console.WriteLine($"Generate item {i}");
            //    }

            //    return list;
            //}
        }
    }
}
