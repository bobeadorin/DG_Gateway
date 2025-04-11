using ApiGatewayService.Models;
using System.Text.Json;

namespace ApiGatewayService.Utilities
{
    public class ConfigUpdates : IConfigUpdates
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ConfigUpdates> _logger;

        public ConfigUpdates(IConfiguration config, ILogger<ConfigUpdates> logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool OcelotIpUpdate(string Ip)
        {
            
            var ocelotFilePath = "ocelot.json"; 
            var ocelotJson = File.ReadAllText(ocelotFilePath);

            var ocelotConfig = JsonSerializer.Deserialize<OcelotConfig>(ocelotJson);

            if (ocelotConfig == null || ocelotConfig.Routes == null)
            {
                _logger.LogError("Failed to deserialize ocelot.json or Routes configuration is null.");
                return false;
            }

            foreach (var route in ocelotConfig.Routes)
            {
                if (route.DownstreamHostAndPorts != null && route.DownstreamHostAndPorts.Count > 0)
                {
                    route.DownstreamHostAndPorts[0].Host = Ip;
                    _logger.LogInformation("Updated DownstreamHostAndPorts.Host to {Ip} for route: {Route}", Ip, route.DownstreamPathTemplate);
                }
            }

            var updatedOcelotJson = JsonSerializer.Serialize(ocelotConfig, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(ocelotFilePath, updatedOcelotJson);

            _logger.LogInformation("ocelot.json file updated successfully.");

            return true;
        }
    }
}
