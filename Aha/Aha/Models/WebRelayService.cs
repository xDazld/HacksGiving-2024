using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//Used these docs https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services?source=recommendations#create-a-socket-server

namespace Aha.Models
{
    public class WebRelayService
    {
        private readonly Socket socket;
        private static WebRelayService _webRelayService = new WebRelayService();
        public const int portNumber = 8080;
        public event Action<string>? MessageReceived;
        private readonly IPEndPoint ipEndPoint;

        private WebRelayService()
        {
            //Dns.GetHostName will need to be removed if we want to test this nonlocally & Dns.GetHostEntry
            //will be replaced with whichever host the service is talking to 
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

            IPAddress ipAddress = ipHostInfo.AddressList[6];

            //Port 8080, defined at the top of file
            ipEndPoint = new IPEndPoint(ipAddress, portNumber);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public static WebRelayService GetWebRelayService()
        {
            return _webRelayService;
        }

        private Socket getSocket()
        {
            return socket;
        }

        private async Task Connect()
        {
            await socket.ConnectAsync(ipEndPoint);
        }

        public async Task WebRelaySendAsync(String message)
        {
            if (!socket.Connected)
            {
                await Connect();
            }

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(messageBytes, SocketFlags.None);
            await receiveTokens();
        }

        private async Task receiveTokens()
        {
            String response = "";
            byte[] buffer = new byte[1024];

            while (!StringToHex(response).Contains("04"))
            {
                if (response.Length > 0)
                {
                    MessageReceived?.Invoke(response);
                }
                int received = await socket.ReceiveAsync(buffer, SocketFlags.None);
                response = Encoding.UTF8.GetString(buffer, 0, received);
            }
            return;
        }

        private string StringToHex(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}