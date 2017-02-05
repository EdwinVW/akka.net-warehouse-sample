using Akka.Actor;
using ColoredConsole;
using System.Collections.Generic;
using System.Linq;
using Messages;
using Actors.Models;
using System;
using Akka.Persistence;
using Actors.Events;
using Newtonsoft.Json.Linq;

namespace Actors
{
    public class SalesActor : ReceivePersistentActor
    {
        private List<Sale> _sales;

        public override string PersistenceId { get;  } = "PersistentSalesActor";

        public SalesActor()
        {
            _sales = new List<Sale>();
            Self.Tell(new DumpSales());

            Command<SellProduct>(cmd => {
                ProductSold productSoldEvent =
                    new ProductSold(cmd.ProductId, cmd.Amount, cmd.TotalPrice);
                Persist(productSoldEvent, Handle);
                return true;
            });

            Command<DumpSales>(cmd => Handle(cmd));

            Recover<JObject>(evt => {
                ProductSold productSold = evt.ToObject<ProductSold>();
                if (productSold != null)
                {
                    Handle(productSold);
                }
            }); 
        }

        private void Handle(ProductSold @event)
        {
            UpdateSales(@event);

            // show sales (only when not recovering from eventstore)
            if (!IsRecovering)
            {
                Self.Tell(new DumpSales());
            }
        }

        private void Handle(DumpSales command)
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

        private void UpdateSales(ProductSold @event)
        {
            var sale = _sales.FirstOrDefault(s => s.ProductId == @event.ProductId);
            if (sale == null)
            {
                sale = new Sale(@event.ProductId, @event.Amount, @event.TotalPrice);
                _sales.Add(sale);
            }
            else
            {
                sale.Increase(@event.Amount, @event.TotalPrice);
            }
        }
    }
}
