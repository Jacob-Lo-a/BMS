
public class Rootobject
{
    public Logging Logging { get; set; }
    public string AllowedHosts { get; set; }
    public Connectionstrings ConnectionStrings { get; set; }
    public Sftpsettings SftpSettings { get; set; }
}

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class Connectionstrings
{
    public string CloudBookstoreDb { get; set; }
}

public class Sftpsettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string RemotePath { get; set; }
}
