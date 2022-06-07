using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class DefaultTFTemplate : TerraformStack
    {
        readonly NameGenerator _nameGenerator;

        internal DefaultTFTemplate(Construct scope, string id, NameGenerator ng) : base(scope, id)
        {
            _nameGenerator = ng;
            Debug.PrintDictContents(_nameGenerator.GetResNames());
            new AzurermProvider(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });

            new VirtualNetwork(this, "vnet", new VirtualNetworkConfig
            {
                Location = _nameGenerator.Region,
                AddressSpace = new[] { "10.0.0.0/16" },
                Name = "TerraformVNet",
                ResourceGroupName = _nameGenerator.GetResNames()["VNetName"]
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
