using PartyStore.Tests;
using PartyStore.Client;
using PartyStore.NetworkModels;

/*
Server server = new Server();
Client client = new Client();

Task t = Task.Run(async () => {
    await server.AcceptClients();
});

await Task.Delay(2000);
await client.ConnectAsync();
*/

MessageClient mc = new MessageClient("localhost", 5101);
await mc.SendMessage("hello", new Tuple<int, int>(2, 3));