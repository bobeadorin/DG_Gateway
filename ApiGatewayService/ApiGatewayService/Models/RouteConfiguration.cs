using Ocelot.Configuration;

namespace ApiGatewayService.Models
{
    public class OcelotConfig
    {
        public GlobalConfiguration GlobalConfiguration { get; set;}
        public List<Routes> Routes { get; set; } 
    }

    public class GlobalConfiguration
    {
        public string BaseUrl { get; set; }
    }

    public class Routes
    {
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<DownstreamHostAndPort> DownstreamHostAndPorts { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
    }
    public class DownstreamHostAndPort
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
