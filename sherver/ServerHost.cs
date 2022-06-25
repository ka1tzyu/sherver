using System.Net;
using System.Net.Sockets;

namespace Server.sherver
{
    public interface IHandler
    {
        void Handle(Stream stream);
    }

    public class StaticFileHandler : IHandler
    {
        public void Handle(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                for (string line = null; line != string.Empty; line = reader.ReadLine())
                {
                    Console.WriteLine(line);
                }
                    
                writer.WriteLine("Server got you");
            }
        }
    }
    
    public class ServerHost
    {
        private readonly IHandler _handler;
        public ServerHost(IHandler handler)
        {
            _handler = handler;
        }

        public void Start()
        {
            var listener = new TcpListener(IPAddress.Any, 80);
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                using (var stream = client.GetStream())
                {
                    _handler.Handle(stream);
                }
            }
        }
    }   
}