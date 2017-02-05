# Akka.NET - Warehouse Sample
This is a sample application demonstrating some aspects of Akka.NET. I use this application for demos during talks I give on Building Actor Model systems using Akka.NET.

This repo contains 3 folders:

| Folder      | Description                                                                                             |
|:------------|:--------------------------------------------------------------------------------------------------------|
| Start       | The base demo with all actors running in 1 single process.                                              |
| Remoting    | The demo with the sales actor running in a remote process (demonstrating Akka.Remoting).                |
| Persistence | The demo with the sales actor persisting state in Azure Table storage (demonstrating Akka.Persistence). | 

## Demo description
This application simulates a central warehouse system with multiple stores that allow customers to scan products in order to purchase them. The system consists of the following parts:

* Inventory - keeps track of the amount of products in stock.
* Sales - keeps track of how many money the company has made on sales.
* Backorder - keeps track of products which need to be back-ordered (because of insufficient stock).
* Store - keeps tracks of scanners being used by customers in the store.
* Scanners - used by customers to purchase products (by scanning them and entering an amount).

## Actor Hierarchy
The following Actor hierarchy is used in the system:

```
                         User
                          |
   -----------------------------------------------
   |                    |             |          |  
   |                    |             |          |
 Store [1..n]       Inventory       Sales     Backorder
   |                    |           
   |                    |
Customer [1..n]      Product [1..n]
   |
   |
Scanner
```

For every store a *StoreActor* is created. The StoreActor creates a *CustomerActor* per customer. Every CustomerActor creates a *ScannerActor* to scan items. In this demo, customers with a scanner are simulated using a repeated scheduled message to *Self* with a random amount of delay-time.

The *InventoryActor* creates a *ProductActor*is created per available product in the inventory. I chose this solution in order to serialize access to the product stock-amount (state) by multiple customers. In the initial design, I did this by letting the single InventoryActor handle this. This worked but introduced a bottle-neck at the InventoryActor (mailbox filled up) and increased the response-times which was bad for the customers. 

The *SalesActor* accumulates all sales. Only products actually sold are added - back-ordered products are not added.

The *BackOrderActor* tracks a list of products that need to be back-ordered because of insufficient stock.

