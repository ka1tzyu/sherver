using Server.sherver;

namespace ConsoleApp.Controllers;

public record User(string Name, string Surname, string Login);

public class UsersController : IController
{
    public User[] Index()
    {
        Thread.Sleep(5);
        return new[]
        {
            new User("Andrii", "Chornovuy", "lometa"),
            new User("Andrii", "Novi", "lometaX"),
            new User("Andrii", "Chem", "lometaY")
        };
    }
    
    public async Task<User[]> IndexAsync()
    {
        await Task.Delay(5);
        return new[]
        {
            new User("Andrii", "Chornovuy", "lometa"),
            new User("Andrii", "Novi", "lometaX"),
            new User("Andrii", "Chem", "lometaY")
        };
    }
}