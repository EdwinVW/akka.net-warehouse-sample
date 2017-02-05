# Akka.NET - Warehouse Sample
This is a sample application demonstrating some aspects of Akka.NET. I use this application for demos during talks I give on Building Actor Model systems using Akka.NET.

This repo contains 3 folders:

| Folder      | Description                                                                                             |
|:------------|:--------------------------------------------------------------------------------------------------------|
| Start       | The base demo with all actors running in 1 single process.                                              |
| Remoting    | The demo with the sales actor running in a remote process (demonstrating *Akka.Remoting*).                |
| Persistence | The demo with the sales actor persisting state in Azure Table storage (demonstrating *Akka.Persistence*). | 

## Demo description
This application simulates a central warehouse system with multiple stores that allow customers to scan products in order to purchase them. The system consists of the following parts:

* **Inventory** - keeps track of the amount of products in stock.
* **Sales** - keeps track of how many money the company has made on sales.
* **Back-order** - keeps track of products which need to be back-ordered (because of insufficient stock).
* **Store** - keeps tracks of scanners being used by customers in the store.
* **Scanners** - represent bar-code scanners used by customers to purchase products (by scanning them and entering an amount).

## Actor Hierarchy
The following Actor hierarchy is used in the system:

![Actor Hierarchy](actor-hierarchy.png)

For every store a *Store* actor is created. The store actor creates a *Customer* actor per customer. Every customer actor creates a *Scanner* actor to represent a bar-code scanner. In this demo, customers with a scanner are simulated by letting the customer actor repeatedly send a scheduled message to *Self* at random intervals.

The *Sales* actor accumulates all sales. Only products actually sold are added - back-ordered products will not show up in the sales report.

The *BackOrder* actor tracks the list of products that must be back-ordered because of insufficient stock.

### Handling concurrency on inventory
Because multiple customers could order the same product at the same time, I needed to serialize access to the products' stock-amount. I solved this problem by letting the *Inventory* actor create a *Product* actor per available product in the inventory. Every product actor has as its state the amount of products in stock.

### Refactoring
In my initial design, I tried to solve the concurrency problem by letting a single inventory actor handle this. While this solution worked, it introduced a performance bottle-neck at the inventory Actor. Its mailbox filled up quite rapidly which decreased the performance of the system. 

This demonstrates that it's important that you experiment with several solutions for your problem or partition your problem in a different way in order to solve it efficiently using an Actor Model approach. My advice would be to always start drawing out the actor hierarchy on a whiteboard (or do some [Event Storming](http://eventstorming.com/)) to validate your design before implementing it.

## Start Solution
The *Start* solution contains the initial version of the demo application. All actors run in a single process. This process is the *Warehouse* console application. When this project is started, the application initializes and waits for a keypress before starting the simulation. 

### Simulating concurrency
By default, only 1 store with 1 customer is simulated. You can increase this by setting the following variables:

* *numberOfStores* in *Program.cs* in the *Warehouse* project. 
* *_numberOfCustomers* in *StoreActor.cs* in the *Actors* project.

### Simulation output
Throughout the simulation, information about what's going on is printed to the console. I've used different colors to identify which actor prints the message:

| Color     | Emitted by        |
|:----------|:------------------|
| White     | Store actor       |
| Yellow    | Scanner actor     |
| Green     | Product actor     |
| Cyan      | Sales actor       |
| Red       | BackOrder actor   |

### Getting started
Make sure you browse the code of the start solution first to see how I've used Akka.NET to build this system. Make sure you're familiar with the structure of this solution before looking at the other sample solutions in this repo.

## Remoting solution
The *Remoting* solution demonstrates location transparency using *Akka.Remote* (see the [documentation](http://getakka.net/docs/remoting)). It contains an additional project *Sales* which is a console application. In this solution, the *Sales* actor will be created in the process of this second console application. Make sure you set both the *Warehouse* project and the *Sales* project as start-up projects. 

### Configuration
The code in this solution is almost identical to the code in the *Start* solution. The magic is primarily in the *App.config* of the console applications. 

In the *Warehouse* project, remoting is configured:

```JSON  
actor { 
  provider = "Akka.Remote.RemoteActorRefProvider, Akka.Remote"    
  
  deployment {   
    /sales {  remote = "akka.tcp://sales@localhost:9999" }               
  }   
}  

remote { 
  helios.tcp {   
    port = 0 # bound to a dynamic port assigned by the OS   
    hostname = localhost 
  }  
}
```
Notice that the actor named */sales* will be created at address: *akka.tcp://sales@localhost:9999*. 

The app.config of the *Sales* project also contains remoting configuration:

```JSON  
actor { 
  provider = "Akka.Remote.RemoteActorRefProvider, Akka.Remote"  
}   

remote { 
  helios.tcp {   
    port = 9999   
    hostname = localhost 
  }  
}
```
Notice that this process will be configured to listen for remoting messages on port *9999*.

The *Sales* actor is changed slightly in this solution to make sure it dumps its sales report to the console after every sale.

## Persistence solution
The *Persistence* solution demonstrates Actor state persistence using *Akka.Persistence* (see the [documentation](http://getakka.net/docs/persistence/architecture)) to enable an actor to store its state and survive restarts. By default, *event-sourcing* is used to store the state of the actor. This means that updating the state will always be done by handling events.

### Peristent Sales actor
For this demo I've chosen to make the *Sales* actor persistent. I could've also chosen the *Product* actor - which would made more sense in real-life. But because the *Sales* actor was already living in a seperate process, I selected that one.

### Storage provider
Akka.Persistence comes with several pluggable storage-providers. For this sample, I've chosen the *Azure Table Storage* provider. This means that you need to have the Azure Storage SDK with the local Storage Emulator installed on your machine in order to run this demo. Another option would be to configure a different storage provider (as described [here](http://getakka.net/docs/persistence/storage-plugins)).  

### Configuration
The structure for this solution is basically the same as the *Remoting* solution with the following changes:

* In the *app.config* of the *Sales* project Akka.Persistence is configured.
* The *Sales* actor derives from *ReceivePersistentActor* to make it persistent.

### Setting up the persistent actor
In the constructor of the *Sales* actor you can see how I've used the *Command* method from the base-class to specify how the actor handles incoming commands by:

1. creating an event based on the command,
2. persist this event and
3. update the state of the actor and handle the event using the *Persist* method from the base-class.

Also the constructor shows how the actor handles recovering from restarts using the *Recover* method from the base-class. In this sample I've used a generic JObject to deserialize the events from storage. 

Notice how I use the convenient *IsRecovering* property from the base-class to determine whether I'm handling a new event as a result of command that came in (*false*) or I'm replaying a historical event (*true*).

### Initializing Azure Table Storage
In the solution I've included a Powershell script *preparepersistence.ps1*. This script does 2 things: 

1. Start the local Azure Storage Emulator.
2. Delete **any** existing tables in development storage.

You can use this script to initialize the environment before running the sample.

## Disclaimer
This is sample code and should be treated as such. I've demonstrated only a small portion of the Akka.NET framework and possibilities. Also I've cut a lot of corners which I wouldn't ever do in production code. 

## Resources
Check out the [Akka.NET documentation](http://getakka.net) to get started with Akka.Net.

If you have some time, I would also advice you to do the [Akka.NET Bootcamp](http://learnakka.net/).
