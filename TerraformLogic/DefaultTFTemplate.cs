using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.TerraformLogic;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class DefaultTFTemplate
    {
        internal DefaultTFTemplate(){}

        internal static void Synthesise(NameGenerator ng)
        {
            HashiCorp.Cdktf.App app = new();
            ResourceGroups resourceGroup = new(app, "ResourceGroup", ng);
            Networking networking = new(app, "Network", ng, resourceGroup);
            Kubernetes kubernetes = new(app, "Kubernetes", ng, networking, resourceGroup);
            StorageAccounts storageAccounts = new(app, "Storage", ng, networking, resourceGroup);
            app.Synth();
        }
    }
}
