using System.Net;
using SCSHost;
using HostStore;
using StoreModels.File;

var fileIndex = FileIndex.Init(options => 
{
    options.RootPath = @"C:\Users\josha\Desktop";
});

var lan = IPAddress.Parse("192.168.1.1");
var wan = IPAddress.Parse("192.168.1.1");
int port = 5050;

var router = new ConnectionListener(lan, wan, port, fileIndex);

int sessionCounter = 0;
Dictionary<int, ISession> sessionMap = new Dictionary<int, ISession>();

CommandLoop();

void CommandLoop()
{
    clearScreen();

    string[] parameters = new string[0];
    while(parameters.Length == 0 || parameters[0] != "exit")
    {
        Console.Write("\n~");
        string? input = Console.ReadLine();
        if (input != null)
            parameters = input.Split(' ');
        else
            continue;


        if (parameters[0] == "help")
            printHelp();
        else if (parameters[0] == "open")
            openSession();
        else if (parameters[0] == "close")
            closeSession(Convert.ToInt32(parameters[1]));
        else if (parameters[0] == "list")
            listSessions();
        else if (parameters[0] == "logs")
            printLogs(Convert.ToInt32(parameters[1]));
        else if (parameters[0] == "clear")
            clearScreen();
    }
}

void printHelp()
{
    Console.WriteLine("open - opens a new session with the returned ID");
    Console.WriteLine("close [id] - closes the session with the specified ID");
    Console.WriteLine("list - lists all available sessions");
    Console.WriteLine("logs [id] - prints the current logs for the specified Id");
    Console.WriteLine("clear - clears the screen");
}

void openSession()
{
    var session = router.OpenSession();
    sessionMap[++sessionCounter] = session;

    Console.WriteLine($"Created session with ID of: {sessionCounter}");
}

void closeSession(int index)
{
    router.CloseSession(sessionMap[index]);
    sessionMap.Remove(index);
}

void listSessions()
{
    var sortedKeys = sessionMap.OrderBy(x => x.Key);
    foreach (var keyValuePair in sortedKeys)
    {
        Console.WriteLine($"Session {keyValuePair.Key}: {keyValuePair.Value.Users.Count()} people connected");
    }
}

void printLogs(int session)
{

}

void clearScreen()
{
    Console.Clear();
    Console.WriteLine("Shared Custom Skins Host");
    Console.WriteLine("Type help for a list of commands or type exit to close");
}
