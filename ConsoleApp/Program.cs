using Server.sherver;

ServerHost host = new ServerHost(new StaticFileHandler());
host.Start();