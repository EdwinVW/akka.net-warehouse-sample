using Akka.Actor;
using ColoredConsole;
using System;
using Messages;

namespace Actors
{
    public class CustomerActor : TypedActor
    {
        private int _storeId;
        private int _userId;
        private string _scannerId;
        private int _scannedItems = 0;  // number of items scanned
        private int _minimalAmountRequired; // minimal amount of items required
        private IActorRef _scannerActor;
        private ActorSelection _storeActor;

        public CustomerActor(int storeId, int userId)
        {
            // setup state
            _storeId = storeId;
            _userId = userId;
            _scannerId = $"scanner{_storeId}-{_userId}";
            _minimalAmountRequired = Randomizer.Next(25, 150);

            // define actors
            var props = Props.Create<ScannerActor>(new object[] { _scannerId });
            _scannerActor = Context.ActorOf(props, _scannerId);
            _storeActor = Context.ActorSelection($"/user/store{_storeId}");

            // activate scanner
            _storeActor.Tell(new ActivateScanner(_scannerId));

            // start scanning immediately
            ScheduleNextScan();
        }

        private void ScheduleNextScan()
        {
            // create a ScanItem message
            int amount = Randomizer.Next(1, 15); // random amount ...
            int productId = Randomizer.Next(1, 26); // ... of a random product
            ScanItem message = new ScanItem(productId, amount);

            // schedule message at a random interval
            int delayInMilliSeconds = Randomizer.Next(500, 1000);
            Context.System.Scheduler.ScheduleTellOnce(
                TimeSpan.FromMilliseconds(delayInMilliSeconds), Self, message, Self);
        }

        public void Handle(ScanItem message)
        {
            _scannerActor.Tell(message);

            // update stats
            _scannedItems += message.Amount;

            // determine progress
            if (_scannedItems < _minimalAmountRequired)
            {
                ScheduleNextScan();
            }
            else
            {
                _storeActor.Tell(new ScanningCompleted(_userId));
            }
        }
    }
}
