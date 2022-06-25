using System.Net;
using System.Net.Sockets;

namespace Server.sherver
{
    public class ServerHost
    {
        private readonly IHandler _handler;
        public ServerHost(IHandler handler)
        {
            _handler = handler;
        }

        public void StartV1()
        {
            Console.WriteLine("Server Started V1");

            try
            {
                var listener = new TcpListener(IPAddress.Any, 80);
                listener.Start();

                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var firstLine = reader.ReadLine();
                        for (string line = null; line != string.Empty; line = reader.ReadLine())
                        {
                        }

                        var request = RequestParser.Parse(firstLine);
                        _handler.Handle(stream, request);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public void StartV2()
        {
            Console.WriteLine("Server Started V2");
            var listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        public async Task StartV3()
        {
            Console.WriteLine("Server Started V3");
            var listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var _ = ProcessClientAsync(client);
            }
        }

        private void ProcessClient(TcpClient client)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    using (client)
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var firstLine = reader.ReadLine();
                        for (string line = null; line != string.Empty; line = reader.ReadLine())
                        {
                        }

                        var request = RequestParser.Parse(firstLine);
                        _handler.Handle(stream, request);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
        
        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                {
                    var firstLine = await reader.ReadLineAsync();
                    for (string line = null; line != string.Empty; line = await reader.ReadLineAsync())
                    {
                    }

                    var request = RequestParser.Parse(firstLine);

                    await _handler.HandleAsync(stream, request);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }   
}