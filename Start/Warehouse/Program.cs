using Akka.Actor;
using System;
using Actors;
using Messages;

namespace Warehouse
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfStores = 1;

            // start system
            using (ActorSystem system = ActorSystem.Create("warehouse"))
            {
                // create child actors
                IActorRef inventoryActor = system.ActorOf<InventoryActor>("inventory");
                IActorRef salesActor = system.ActorOf<SalesActor>("sales");
                IActorRef backorderActor = system.ActorOf<BackorderActor>("backorder");

                Console.WriteLine("Press any key to open the stores...");
                Console.ReadKey(true);
                
                // start store simulation
                for (int i = 0; i < numberOfStores; i++)
                {
                    Props props = Props.Create<StoreActor>(new object[] { i });
                    system.ActorOf(props, $"store{i}");
                }

                Console.ReadKey(true); // keep the actorsystem alive

                // dump backorders
                Console.WriteLine("\nDumping sales");
                salesActor.Tell(new DumpSales());
                Console.ReadKey(true);

                // dump backorders
                Console.WriteLine("\nDumping backorders");
                backorderActor.Tell(new DumpBackorders());
                Console.ReadKey(true);
            }
        }
    }
}
