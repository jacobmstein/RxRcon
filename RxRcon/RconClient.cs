using System.Reactive.Linq;
using System.Text.Json;
using WebSocketSharp;

namespace RxRcon;

public class RconClient : IDisposable
{
    private readonly WebSocket _webSocket;

    public RconClient(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException();

        _webSocket = new WebSocket(uri);

        Messages = Observable
            .FromEventPattern<MessageEventArgs>(x => _webSocket.OnMessage += x,
                x => _webSocket.OnMessage -= x)
            .Select(e => JsonSerializer.Deserialize<RconMessage>(e.EventArgs.Data)!)
            .Publish()
            .RefCount();
    }

    public RconClient(string ip, int port, string password) : this($"ws://{ip}:{port}/{password}")
    {
    }

    public IObservable<RconMessage> Messages { get; set; }

    public void Dispose()
    {
        ((IDisposable) _webSocket).Dispose();
    }

    public void Connect()
    {
        _webSocket.Connect();
    }

    public void Send(string command, int identifier = 0)
    {
        _webSocket.Send(JsonSerializer.Serialize(new
        {
            Identifier = identifier,
            Message = command,
            Name = "WebRcon"
        }));
    }
}