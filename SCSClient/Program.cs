using SCSClient;
using SCSClient.UI;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<EventStore>();
builder.Services.AddSingleton<ActionThread>();
builder.Services.AddSingleton<DataResponseThread>();


var app = builder.Build();
app.UseWebSockets();
app.MapControllers();

app.Run();
