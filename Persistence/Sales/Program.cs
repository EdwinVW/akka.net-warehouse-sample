using Akka.Actor;
using System;

namespace Sales
{
    class Program
    {
        static void Main(string[] args)
        {
            // start system
            using (ActorSystem system = ActorSystem.Create("sales"))
            {
                Console.WriteLine("Sales system online. Press any key to stop");

                Console.ReadKey(true);
            }
        }
    }
}
