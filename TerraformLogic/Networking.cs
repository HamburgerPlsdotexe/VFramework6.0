using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.TerraformLogic
{
    internal class Networking : TerraformStack
    {
        internal readonly VirtualNetwork _virtualNetwork;
        internal readonly Subnet _subnet;
        internal readonly PrivateDnsZone _privateDnsZone;
        internal readonly PrivateEndpoint _privateEndpoint;
        internal readonly PrivateDnsZoneVirtualNetworkLink _privateDnsZoneVirtualNetworkLink;

        internal readonly ResourceGroup _resourceGroup;

        internal Networking(Construct scope, string id, NameGenerator ng, ResourceGroups rg) : base(scope, id)
        {
            AzurermProvider azurermProvider = new(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });

            VirtualNetwork virtualNetwork = new (this, "azurerm_virtual_network", new VirtualNetworkConfig
            {
                Name = ng.GetResNames()["VNetName"],
                Location = ng.Region[1],
                ResourceGroupName = rg._resourceGroup.Name,
                AddressSpace = new[] { "10.0.0.0/16" },
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            Subnet subnet = new (this, "azurerm_subnet", new SubnetConfig
            {
                Name = ng.GetResNames()["VNetSubnet1"],
                AddressPrefixes = new string[] { "aks" },
                VirtualNetworkName = virtualNetwork.Name,
                ResourceGroupName = rg._resourceGroup.Name,
                EnforcePrivateLinkEndpointNetworkPolicies = true,
            });

            PrivateDnsZone privateDnsZone = new (this, "azurerm_private_dns_zone", new PrivateDnsZoneConfig
            {
                Name = "privatelink.file.core.windows.net",
                ResourceGroupName = rg._resourceGroup.Name
            });

             PrivateDnsZoneVirtualNetworkLink privateDnsZoneVirtualNetworkLink = new (this, "azurerm_private_dns_zone_virtual_network_link", new PrivateDnsZoneVirtualNetworkLinkConfig
            {
                Name = ng.EnvironmentName("long", env: "env") + "",
                ResourceGroupName = rg._resourceGroup.Name,
                PrivateDnsZoneName = privateDnsZone.Name,
                VirtualNetworkId = virtualNetwork.Id
            });

            PrivateEndpoint privateEndpoint = new PrivateEndpoint(this, "azurerm_private_endpoint", new PrivateEndpointConfig
            {
                Name = ng.EnvironmentName("long", env: "env"),
                Location = ng.Region[1],
                ResourceGroupName = rg._resourceGroup.Name,
                SubnetId = subnet.Id,

                PrivateDnsZoneGroup = new PrivateEndpointPrivateDnsZoneGroup
                {
                    Name = "azurerm_private_dns_zone.private-dns-zone",
                    PrivateDnsZoneIds = new string[] { privateDnsZone.Id }
                },

                PrivateServiceConnection = new PrivateEndpointPrivateServiceConnection
                {
                    Name = ng.EnvironmentName("long", env: "env") + "-private-service-connection-file-share",
                    PrivateConnectionResourceId = "",
                    IsManualConnection = false,
                }
            });

            _privateDnsZone = privateDnsZone;
            _privateDnsZoneVirtualNetworkLink = privateDnsZoneVirtualNetworkLink;
            _privateEndpoint = privateEndpoint;
            _subnet = subnet;
            _virtualNetwork = virtualNetwork;

        }
    }
}
