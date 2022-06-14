using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.TerraformLogic
{
    internal class ResourceGroups : TerraformStack
    {
        internal readonly ResourceGroup _resourceGroup;

        internal ResourceGroups(Construct scope, string id, NameGenerator ng) : base(scope,id)
        {
            AzurermProvider azurermProvider = new(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });

            ResourceGroup resourceGroup = new ResourceGroup(this, "azurerm_resource_group", new ResourceGroupConfig
            {
                Name = ng.GetResNames()["RgName"],
                Location = ng.Region[1],
                Tags = new Dictionary<string, string> {
                    { "application", ng.EnvironmentName("long")},
                    {"environment", ng.EnvironmentName("long", env:"env") }
                }
            });

            _resourceGroup = resourceGroup;
           
        }
    }
}
