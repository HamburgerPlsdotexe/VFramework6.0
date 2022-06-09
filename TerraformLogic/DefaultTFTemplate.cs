using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class DefaultTFTemplate : TerraformStack
    {
        internal DefaultTFTemplate(Construct scope, string id, NameGenerator ng) : base(scope, id)
        {
            new AzurermProvider(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });
            
            new ResourceGroup(this, "azurerm_resource_group", new ResourceGroupConfig
            {
                Name = ng.GetResNames()["RgName"],
                Location = ng.Region[1],
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            new VirtualNetwork(this, "azurerm_virtual_network", new VirtualNetworkConfig
            {
                Id = "5",
                Name = ng.GetResNames()["VNetName"],
                Location = ng.Region[1],
                ResourceGroupName = ng.GetResNames()["RgName"],
                AddressSpace = new[] { "10.0.0.0/16" },
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            new Subnet(this, "azurerm_subnet", new SubnetConfig
            {
                Name = ng.GetResNames()["VNetSubnet1"],
                AddressPrefixes = new string[] {""},
                VirtualNetworkName = ng.GetResNames()["VNetName"],
                ResourceGroupName = ng.GetResNames()["RgName"],
                EnforcePrivateLinkEndpointNetworkPolicies = true,
            });

            new PrivateDnsZone(this, "azurerm_private_dns_zone", new PrivateDnsZoneConfig
            {
                Name = "privatelink.file.core.windows.net",
                ResourceGroupName = ng.GetResNames()["RgName"]
            });

            new PrivateDnsZoneVirtualNetworkLink(this, "azurerm_private_dns_zone_virtual_network_link", new PrivateDnsZoneVirtualNetworkLinkConfig
            {
                Name = ng.EnvironmentName("long", env: "env") + "",
                ResourceGroupName = ng.GetResNames()["RgName"],
                PrivateDnsZoneName = "azurerm_private_dns_zone.private-dns-zone",
                VirtualNetworkId = "azurerm_virtual_network.Id"
            });

            new PrivateEndpoint(this, "azurerm_private_endpoint", new PrivateEndpointConfig
            {
                Name = ng.EnvironmentName("long", env: "env"),
                Location = ng.Region[1],
                ResourceGroupName = ng.GetResNames()["RgName"],
                SubnetId = "azurerm_subnet.services-network-subnet.id",

                PrivateDnsZoneGroup = new PrivateEndpointPrivateDnsZoneGroup
                {
                    Name = "azurerm_private_dns_zone.private-dns-zone",
                    PrivateDnsZoneIds = new string[] { "privatelink.file.core.windows.net" }
                },

                PrivateServiceConnection = new PrivateEndpointPrivateServiceConnection
                {
                    Name = ng.EnvironmentName("long", env:"env") + "-private-service-connection-file-share",
                    IsManualConnection = false,
                }
            });
        }

        internal static void Init(NameGenerator ng)
        {
            HashiCorp.Cdktf.App app = new();
            new DefaultTFTemplate(app, "azure", ng);
            app.Synth();
        }
    }
}
