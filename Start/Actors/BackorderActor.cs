using Akka.Actor;
using ColoredConsole;
using System.Collections.Generic;
using System.Linq;
using Messages;
using Actors.Models;

namespace Actors
{
    public class BackorderActor : TypedActor, IHandle<BackorderProduct>, IHandle<DumpBackorders>
    {
        private List<Backorder> _backorders;

        public BackorderActor()
        {
            _backorders = new List<Backorder>();
        }

        public void Handle(BackorderProduct message)
        {
            UpdateState(message);

            ColorConsole.WriteLine($"Backorder {message.Amount} units of product {message.ProductId}".Red());
        }

        public void Handle(DumpBackorders message)
        {
            ColorConsole.WriteLine("Backorder overview:".Red());
            ColorConsole.WriteLine("===================".Red());
            foreach (var backorder in _backorders.OrderBy(b => b.ProductId))
            {
                ColorConsole.WriteLine($"Product {backorder.ProductId} : {backorder.Amount} units.".Red());
            }
        }

        private void UpdateState(BackorderProduct message)
        {
            var backorder = _backorders.FirstOrDefault(b => b.ProductId == message.ProductId);
            if (backorder == null)
            {
                backorder = new Backorder(message.ProductId, message.Amount);
                _backorders.Add(backorder);
            }
            else
            {
                backorder.Increase(message.Amount);
            }
        }
    }
}
