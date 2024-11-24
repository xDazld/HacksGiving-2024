using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

//Used these docs https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services?source=recommendations#create-a-socket-server

namespace Aha.Models
{
    public class WebRelayService
    {
        private readonly Socket socket;
        private static WebRelayService _webRelayService = new WebRelayService();
        public const int portNumber = 443;
        public event Action<string>? MessageReceived;
        public event Action<string>? MessageEnd;
        private readonly IPEndPoint ipEndPoint;

        private WebRelayService()
        {
            //Dns.GetHostName will need to be removed if we want to test this nonlocally & Dns.GetHostEntry
            //will be replaced with whichever host the service is talking to 
            string hostName = "o-chi.pcr.dog";
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

            IPAddress ipAddress = ipHostInfo.AddressList[0];

            //Port, defined at the top of file
            ipEndPoint = new IPEndPoint(ipAddress, portNumber);
            //Create a socket that is connected to https://o-chi.pcr.dog/HacksGiving-2024 on port 443

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
            int bytesSent = await socket.SendAsync(messageBytes, SocketFlags.None);        
            await receiveTokens();
        }

        private async Task receiveTokens()
        {
            String response = "";
            byte[] buffer = new byte[1024];

            while (!StringToHex(response).Contains("04"))
            {
                if(response.Length > 0)
                {
                    MessageReceived?.Invoke(response);
                }
                int received = await socket.ReceiveAsync(buffer, SocketFlags.None);
                response = Encoding.UTF8.GetString(buffer, 0, received);
            }
            MessageEnd?.Invoke(""); //TODO May be able to remove this if we are awaiting the send method. This is because when it returns we can set currentResponseMessage to null
            return;
        }

        private string StringToHex(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}