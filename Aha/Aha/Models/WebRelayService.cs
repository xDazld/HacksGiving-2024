using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//Used these docs https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services?source=recommendations#create-a-socket-server


namespace Aha.Models;

    internal class WebRelayService
{

    // Static field to hold the single instance of the client
    private static Socket? _instance;

        private static readonly int portNumber = 8080;

    IPEndPoint? ipEndPoint;

    // Lock for thread safety when initializing the singleton
    private static readonly object _socket_lock = new object();

    // Private constructor to prevent instantiation outside the class. Enforces singleton pattern.
    private Socket getSocket()
    {
        if (_instance != null)
        {
            return _instance;
        }

        // Create the socket client

        lock (_socket_lock)
        {
            if (_instance == null)
            {

                //Dns.GetHostName will need to be removed if we want to test this nonlocally & Dns.GetHostEntry
                //will be replaced with whichever host the service is talking to 
                string hostName = Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

                IPAddress ipAddress = ipHostInfo.AddressList[6];

                //Port 8080, defined at the top of file
                ipEndPoint = new IPEndPoint(ipAddress, portNumber);
                _instance = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
        }
        return _instance;
    }


    internal async Task WebRelaySendAsync(string message)
    {
        Socket client = getSocket();
        await client.ConnectAsync(ipEndPoint);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
        var buffer = new byte[1_024];
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        client.Shutdown(SocketShutdown.Both);
    }

    internal async Task WebRelayReceiveAsync()
    {
        Socket listener = getSocket();
        listener.Bind(ipEndPoint);
        listener.Listen(100);
        var handler = await listener.AcceptAsync();

        // Receive message.
        var buffer = new byte[1_024];
        var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);

        Debug.Print("Response: " + response + "\n");

    }
}

