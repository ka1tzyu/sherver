using System.Net;

namespace Server.sherver;

public class StaticFileHandler : IHandler
{
    private readonly string _path;
    
    public StaticFileHandler(string path)
    {
        this._path = path;
    }
    
    public void Handle(Stream networkStream, Request request)
    {
        using (var writer = new StreamWriter(networkStream))
        {
            var filePath = Path.Combine(_path, request.Path.Substring(1));

            if (!File.Exists(filePath))
            {
                ResponseWriter.WriteStatus(HttpStatusCode.NotFound, networkStream);
            }
            else
            {
                ResponseWriter.WriteStatus(HttpStatusCode.OK, networkStream);
                using (var fileStream = File.OpenRead(filePath))
                {
                    fileStream.CopyTo(networkStream);
                }
            }
        }
    }
}