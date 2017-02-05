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

