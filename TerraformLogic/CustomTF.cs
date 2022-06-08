using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class CustomTF : TerraformStack
    {
        internal CustomTF(Construct scope, string id, NameGenerator ng) : base(scope, id)
        {
            new AzurermProvider(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures()
            });

            new VirtualNetwork(this, "TfVnet", new VirtualNetworkConfig
            {
                Location = "westeuroe",
                AddressSpace = new[] { "10.0.0.0/24" },
                Name = "TerraformVNet",
                ResourceGroupName = "RGtest"
            });
        }

        internal static void Init(NameGenerator ng)
        {
            HashiCorp.Cdktf.App app = new();
            new CustomTF(app, "azure", ng);
            app.Synth(); 
        }
    }
}
