using System;
namespace targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            welcome8937();
            welcome7795();
            Console.ReadKey();

        }
        static partial void welcome7795();

        private static void welcome8937()
        {
            Console.WriteLine("Enter yoyr name:");
            string userName = Console.ReadLine();
            Console.WriteLine("{0},welcome to my first console application", userName);
        }
    }

}