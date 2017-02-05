using Akka.Actor;
using ColoredConsole;
using Messages;

namespace Actors
{
    public class StoreActor : ReceiveActor
    {
        private int _storeId;
        private int _numberOfCustomers = 1;
        private int _activeScanners = 0;

        public StoreActor(int storeId)
        {
            _storeId = storeId;

            // setup message-handling
            Receive<ActivateScanner>(msg => Handle(msg));
            Receive<ScanningCompleted>(msg => Handle(msg));

            for (int i = 0; i < _numberOfCustomers; i++)
            {
                Props props = Props.Create<CustomerActor>(new object[] { _storeId, i });
                Context.ActorOf(props, $"user{_storeId}-{i}");
            }
        }

        private void Handle(ActivateScanner message)
        {
            _activeScanners++;
            ColorConsole.WriteLine($"Scanner #{message.ScannerId} activated.".White());
        }

        private void Handle(ScanningCompleted message)
        {
            ColorConsole.WriteLine($"Scanner #{message.ScannerId} returned.".White());

            _activeScanners--;
            if (_activeScanners == 0)
            {
                ColorConsole.WriteLine($"All scanners in store {_storeId} have been returned.".White());
            }
        }
    }
}
