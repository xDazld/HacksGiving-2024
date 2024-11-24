using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

//Used these docs https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services?source=recommendations#create-a-socket-server

namespace Aha.Models
{
    public class WebRelayService
    {
        private readonly ClientWebSocket socket;
        private static WebRelayService _webRelayService = new WebRelayService();
        public const int portNumber = 443;
        public event Action<string>? MessageReceived;
        public event Action<string>? MessageEnd;
        private Boolean connected = false;
        //private readonly IPEndPoint ipEndPoint;

        //private WebRelayService()
        //{
        //    //Dns.GetHostName will need to be removed if we want to test this nonlocally & Dns.GetHostEntry
        //    //will be replaced with whichever host the service is talking to 
        //    string hostName = "o-chi.pcr.dog";
        //    IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

        //    IPAddress ipAddress = ipHostInfo.AddressList[0];

        //    //Port, defined at the top of file
        //    ipEndPoint = new IPEndPoint(ipAddress, portNumber);
        //    //Create a socket that is connected to https://o-chi.pcr.dog/HacksGiving-2024 on port 443

        //    socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //}
        private const string ApiHost = "149.130.216.48";
        private const int Port = 443;
        private const string WebSocketPath = "/HacksGiving-2024/ws/12345";
        private readonly Uri webSocketUri;

        private WebRelayService()
        {
            // Construct the WebSocket URI
            webSocketUri = new Uri("wss://o-chi.pcr.dog/HacksGiving-2024/ws/12345678");

            // Initialize the socket
            socket = new();
        }

        public static WebRelayService GetWebRelayService()
        {
            return _webRelayService;
        }

        private ClientWebSocket getSocket()
        {
            return socket;
        }

        private async Task Connect()
        {
            if (!connected)
            {

                // Establish TCP connection
                await socket.ConnectAsync(webSocketUri, default);

                //// Perform WebSocket handshake
                //await PerformWebSocketHandshakeAsync();
            }
            connected = true;
        }

        //private async Task PerformWebSocketHandshakeAsync()
        //{
        //    // Build WebSocket handshake request
        //    var handshakeRequest = new StringBuilder();
        //    handshakeRequest.AppendLine($"GET {webSocketUri.Host}{webSocketUri.PathAndQuery} HTTP/1.1");
        //    handshakeRequest.AppendLine($"Host: {webSocketUri.Host}:{webSocketUri.Port}");
        //    handshakeRequest.AppendLine("Upgrade: websocket");
        //    handshakeRequest.AppendLine("Connection: Upgrade");
        //    handshakeRequest.AppendLine("Sec-WebSocket-Key: x3JJHMbDL1EzLkh9GBhXDw==");
        //    handshakeRequest.AppendLine("Sec-WebSocket-Version: 13");
        //    handshakeRequest.AppendLine();

        //    // Send handshake request
        //    byte[] requestBytes = Encoding.UTF8.GetBytes(handshakeRequest.ToString());
        //    await socket.SendAsync(requestBytes, WebSocketMessageType.Text, true, CancellationToken.None);

        //    // Read handshake response
        //    byte[] responseBuffer = new byte[1024];
        //    var responseResult = await socket.ReceiveAsync(responseBuffer, CancellationToken.None);
        //    string response = Encoding.UTF8.GetString(responseBuffer, 0, responseResult.Count);
        //    return;
        //}

        public async Task WebRelaySendAsync(String message)
        {
            if (!connected)
            {
                await Connect();
            }

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(messageBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            await receiveTokens();
        }

        private async Task receiveTokens()
        {
            String response = "";
            byte[] buffer = new byte[1024];
            int count = 0;
            while (!response.Contains("END CHAT"))
            {
                if (response.Length > 0)
                {
                    MessageReceived?.Invoke(response);
                }
                var received = await socket.ReceiveAsync(buffer, CancellationToken.None);
                response = Encoding.UTF8.GetString(buffer, 0, received.Count);
                count++;
            }
            MessageEnd?.Invoke(""); //TODO May be able to remove this if we are awaiting the send method. This is because when it returns we can set currentResponseMessage to null
            return;
        }
    }
}