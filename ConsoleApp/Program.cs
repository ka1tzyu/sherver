using Server.sherver;

//ServerHost host = new ServerHost(new StaticFileHandler(Path.Combine(Environment.CurrentDirectory, "www")));
ServerHost host = new ServerHost(new ControllersHandler(typeof(Program).Assembly));
host.Start();