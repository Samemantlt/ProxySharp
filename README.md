# ProxySharp
C# Library that allows to use proxies easy&amp;fast

![Nuget](https://img.shields.io/nuget/v/ProxySharp.Tcp)
![Nuget](https://img.shields.io/nuget/dt/ProxySharp.Tcp)

## Example:
```csharp
IConnectionProvider connectionProvider = new Socks4ConnectionProvider("1.2.3.4", 1080);
TcpClient tcpClient = await connectionProvider.Connect("google.com", 80);

// tcpClient is connected to 'google.com:80' through socks4 proxy '1.2.3.4:1080'
```

## TCP Proxies
 - [x] Socks4 (without authorization for now)
 - [ ] Socks4A
 - [ ] Socks5

## HTTP Proxies
 - [ ] HTTP
 - [ ] HTTPS

## UDP Proxies
 - [ ] Socks5
