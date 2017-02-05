using Akka.Actor;
using ColoredConsole;
using System.Collections.Generic;
using System.Linq;
using Messages;
using Actors.Models;
using System;

namespace Actors
{
    public class SalesActor : TypedActor, IHandle<SellProduct>, IHandle<DumpSales>
    {
        private List<Sale> _sales;

        public SalesActor()
        {
            _sales = new List<Sale>();
        }

        public void Handle(SellProduct message)
        {
            UpdateState(message);
        }

        public void Handle(DumpSales message)
        {
            Console.Clear();
            ColorConsole.WriteLine($"Sales overview ({DateTime.Now.ToString("HH:mm:ss")}):".Cyan());
            ColorConsole.WriteLine("==========================".Cyan());
            decimal totalSales = 0;
            foreach (var sale in _sales.OrderBy(s => s.ProductId))
            {
                ColorConsole.WriteLine($"Product {sale.ProductId} : {sale.TotalAmount} units - {sale.TotalPrice} Euro.".Cyan());
                totalSales += sale.TotalPrice;
            }
            ColorConsole.WriteLine($"Total: {totalSales} Euro".Cyan());
        }

        private void UpdateState(SellProduct message)
        {
            var sale = _sales.FirstOrDefault(s => s.ProductId == message.ProductId);
            if (sale == null)
            {
                sale = new Sale(message.ProductId, message.Amount, message.TotalPrice);
                _sales.Add(sale);
            }
            else
            {
                sale.Increase(message.Amount, message.TotalPrice);
            }
            Self.Tell(new DumpSales());
        }
    }
}
