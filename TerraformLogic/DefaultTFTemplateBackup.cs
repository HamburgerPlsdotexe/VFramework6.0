/*using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class DefaultTFTemplate : TerraformStack
    {
        string[] _ips = new string[] { "111.111.111.000", "111.111.111.001" };

        internal DefaultTFTemplate(Construct scope, string id, NameGenerator ng) : base(scope, id)
        {
            new AzurermProvider(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });
            
            ResourceGroup RG = new ResourceGroup(this, "azurerm_resource_group", new ResourceGroupConfig
            {
                Name = ng.GetResNames()["RgName"],
                Location = ng.Region[1],
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            StorageAccount SA = new StorageAccount(this, "azurerm_storage_account", new StorageAccountConfig
            {
                Name = "test",
                ResourceGroupName = ng.GetResNames()["RgName"],
                Location = ng.Region[1],

                AccountTier = "Standard",
                AccountKind = "StorageV2",
                AccountReplicationType = "RAGRS",
                AllowBlobPublicAccess = true,

                NetworkRules = new StorageAccountNetworkRules
                {
                    DefaultAction = "Deny",
                    VirtualNetworkSubnetIds = new string[] { "durrr" },
                    IpRules = _ips
                }
            });

            StorageShare SH = new StorageShare(this, "azurerm_storage_share", new StorageShareConfig
            {
                Name = "authtickets",
                StorageAccountName = SA.Name,
                Quota = 5
            });

            VirtualNetwork Vnet = new VirtualNetwork(this, "azurerm_virtual_network", new VirtualNetworkConfig
            {
                Name = ng.GetResNames()["VNetName"],
                Location = ng.Region[1],
                ResourceGroupName = RG.Name,
                AddressSpace = new[] { "10.0.0.0/16" },
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            Subnet Sbnet = new Subnet(this, "azurerm_subnet", new SubnetConfig
            {
                Name = ng.GetResNames()["VNetSubnet1"],
                AddressPrefixes = new string[] {"aks"},
                VirtualNetworkName = Vnet.Name,
                ResourceGroupName = RG.Name,
                EnforcePrivateLinkEndpointNetworkPolicies = true,
            });

            PrivateDnsZone Pdz = new PrivateDnsZone(this, "azurerm_private_dns_zone", new PrivateDnsZoneConfig
            {
                Name = "privatelink.file.core.windows.net",
                ResourceGroupName = RG.Name
            });

            new PrivateDnsZoneVirtualNetworkLink(this, "azurerm_private_dns_zone_virtual_network_link", new PrivateDnsZoneVirtualNetworkLinkConfig
            {
                Name = ng.EnvironmentName("long", env: "env") + "",
                ResourceGroupName = RG.Name,
                PrivateDnsZoneName = Pdz.Name,
                VirtualNetworkId = Vnet.Id
            });

            PrivateEndpoint Pe = new PrivateEndpoint(this, "azurerm_private_endpoint", new PrivateEndpointConfig
            {
                Name = ng.EnvironmentName("long", env: "env"),
                Location = ng.Region[1],
                ResourceGroupName = RG.Name,
                SubnetId = Sbnet.Id,

                PrivateDnsZoneGroup = new PrivateEndpointPrivateDnsZoneGroup
                {
                    Name = "azurerm_private_dns_zone.private-dns-zone",
                    PrivateDnsZoneIds = new string[] { Pdz.Id }
                },

                PrivateServiceConnection = new PrivateEndpointPrivateServiceConnection
                {
                    Name = ng.EnvironmentName("long", env:"env") + "-private-service-connection-file-share",
                    PrivateConnectionResourceId = SA.Id,
                    IsManualConnection = false,
                }
            });

            KubernetesCluster K8sCluster = new KubernetesCluster(this, "azurerm_kubernetes_cluster", new KubernetesClusterConfig
            {
                Name = ng.GetResNames()["K8sName"],
                Location = ng.Region[1],
                DnsPrefix = "aks",
                KubernetesVersion = "1.19.11",
                ResourceGroupName = RG.Name,
                
                Identity = new KubernetesClusterIdentity 
                {
                    Type = "SystemAssigned",
                },

                OmsAgent = new KubernetesClusterOmsAgent
                {
                    LogAnalyticsWorkspaceId = ""
                },

                DefaultNodePool = new KubernetesClusterDefaultNodePool
                {
                    Name = "systemnp",
                    VmSize = "Standard_B2MS",
                    VnetSubnetId = Vnet.Id,
                    OsDiskSizeGb = 50,
                    EnableAutoScaling = true,
                    MinCount = 1,
                    MaxCount = 2,
                    MaxPods = 10,
                    OrchestratorVersion = "1.19.11",
                    AvailabilityZones = new string[] {"1","2","3"}
                }
            });

            new KubernetesClusterNodePool(this, "azurerm_kubernetes_cluster_node_pool", new KubernetesClusterNodePoolConfig
            {
                Name = "winnp",
                OsType = "Windows",
                KubernetesClusterId = K8sCluster.Id,
                VnetSubnetId = Vnet.Id,
                VmSize = "Standard_B2MS",
                EnableAutoScaling = true,
                OsDiskSizeGb = 50,
                MinCount = 1,
                MaxCount = 2,
                MaxPods = 10,
                OrchestratorVersion = "1.19.11"
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
*/