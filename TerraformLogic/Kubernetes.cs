using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.TerraformLogic
{
    internal class Kubernetes : TerraformStack
    {
        internal readonly KubernetesCluster _kubernetesCluster;
        internal readonly KubernetesClusterNodePool _kubernetesClusterNodePool;

        internal Kubernetes(Construct scope, string id, NameGenerator ng, Networking nw, ResourceGroups rg) : base(scope, id)
        {
            AzurermProvider azurermProvider = new(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });

            KubernetesCluster kubernetesCluster = new(this, "azurerm_kubernetes_cluster", new KubernetesClusterConfig
            {
                Name = ng.GetResNames()["K8sName"],
                Location = ng.Region[1],
                DnsPrefix = "aks",
                KubernetesVersion = "1.19.11",
                ResourceGroupName = rg._resourceGroup.Name,

                Identity = new KubernetesClusterIdentity
                {
                    Type = "SystemAssigned",
                },

                DefaultNodePool = new KubernetesClusterDefaultNodePool
                {
                    Name = "systemnp",
                    VmSize = "Standard_B2MS",
                    VnetSubnetId = nw._virtualNetwork.Id,
                    OsDiskSizeGb = 50,
                    EnableAutoScaling = true,
                    MinCount = 1,
                    MaxCount = 2,
                    MaxPods = 10,
                    OrchestratorVersion = "1.19.11",
                    AvailabilityZones = new string[] { "1", "2", "3" }
                }
            });

            KubernetesClusterNodePool kubernetesClusterNodePool = new(this, "azurerm_kubernetes_cluster_node_pool", new KubernetesClusterNodePoolConfig
            {
                Name = "winnp",
                OsType = "Windows",
                KubernetesClusterId = kubernetesCluster.Id,
                VnetSubnetId = nw._virtualNetwork.Id,
                VmSize = "Standard_B2MS",
                EnableAutoScaling = true,
                OsDiskSizeGb = 50,
                MinCount = 1,
                MaxCount = 2,
                MaxPods = 10,
                OrchestratorVersion = "1.19.11"
            });

            _kubernetesCluster = kubernetesCluster;
            _kubernetesClusterNodePool = kubernetesClusterNodePool;

        }
    }
}

