using System;
using OsuCSharp.DataStructures;

namespace OsuCSharp.ConsolePlayground
{
    class Program
    {
        static void Main()
        {
            LinkedList<int> linkedList = new LinkedList<int>();

            Random rnd = new Random();

            for (int i = 0; i < 50; i++)
            {
                linkedList.AddLast(rnd.Next(1, 101));
            }

            Console.ReadLine();
        }
    }
}
