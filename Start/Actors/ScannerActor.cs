using Akka.Actor;
using ColoredConsole;
using System;
using Messages;

namespace Actors
{
    public class ScannerActor : TypedActor, IHandle<ScanItem>
    {
        private string _scannerId;
        private IActorRef _storeActor;

        public ScannerActor(string scannerId)
        {
            // setup state
            _scannerId = scannerId;

            // define other actors
            _storeActor = Context.Parent;
        }

        public void Handle(ScanItem message)
        {
            // purchase product
            string productPath = $"/user/inventory/product-{message.ProductId}";
            Context.ActorSelection(productPath)
                .Tell(new PurchaseProduct(message.ProductId, message.Amount));

            // show status
            ColorConsole.WriteLine($"Scanner #{_scannerId} : scanned {message.Amount} units of product {message.ProductId}.".Yellow());
        }
    }
}
