using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Server.sherver;

public class ControllersHandler : IHandler
{
    private readonly Dictionary<string, Func<object>> _routes;
    
    public ControllersHandler(Assembly controllersAssembly)
    {
        this._routes = controllersAssembly.GetTypes()
            .Where(x => typeof(IController).IsAssignableFrom(x))
            .SelectMany(Controller => Controller.GetMethods().Select(Method => new {
                    Controller, 
                    Method
                })
            ).ToDictionary(
                key => GetPath(key.Controller, key.Method), 
                value => GetEndpointMethod(value.Controller, value.Method)
            );
    }

    private Func<object?> GetEndpointMethod(Type controller, MethodInfo method)
    {
        return () => method.Invoke(Activator.CreateInstance(controller), Array.Empty<object>());
    }

    private string GetPath(Type controller, MethodInfo method)
    {
        string name = controller.Name;
        if (name.EndsWith("controller", StringComparison.InvariantCultureIgnoreCase)) 
            name = name.Substring(0, name.Length - "controller".Length);
        if (method.Name.Equals("Index", StringComparison.CurrentCultureIgnoreCase))
            return "/" + name;
        return "/" + name + "/" + method.Name;
    }

    public void Handle(Stream networkStream, Request request)
    {
        if (!_routes.TryGetValue(request.Path, out var func))
            ResponseWriter.WriteStatus(HttpStatusCode.NotFound, networkStream);
        else
        {
            ResponseWriter.WriteStatus(HttpStatusCode.OK, networkStream);
            WriteControllerResponse(func(), networkStream);
        }
    }

    private void WriteControllerResponse(object response, Stream networkStream)
    {
        if (response is string str)
        {
            using var writer = new StreamWriter(networkStream, leaveOpen:true);
            writer.Write(str);
        } 
        else if (response is byte[] buffer)
        {
            networkStream.Write(buffer, 0, buffer.Length);    
        }
        else
        {
            WriteControllerResponse(JsonConvert.SerializeObject(response), networkStream);
        }
    }
}