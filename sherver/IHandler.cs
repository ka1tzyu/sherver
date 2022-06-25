namespace Server.sherver;

public interface IHandler
{
    void Handle(Stream networkStream, Request request);
}