namespace MazeCore.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
