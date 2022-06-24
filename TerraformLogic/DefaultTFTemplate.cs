using Constructs;
using HashiCorp.Cdktf;
using HashiCorp.Cdktf.Providers.Azurerm;
using VFBlazor6._0.TerraformLogic;
using VFBlazor6._0.Utility;

namespace VFBlazor6._0.Terraform
{
    public class DefaultTFTemplate
    {
        internal DefaultTFTemplate() { }
        /// <summary>
        /// The function responsible for synthesising/ generating Terraform JSON files
        /// based on the objects of the resource classes
        /// </summary>
        /// <param name="nameGen">an object of class NameGenerator</param>
        internal static void Synthesise(NameGenerator nameGen)
        {
            HashiCorp.Cdktf.App app = new();
            ResourceGroups resourceGroup = new(app, "ResourceGroup", nameGen);
            Networking networking = new(app, "Network", nameGen, resourceGroup);
            Kubernetes kubernetes = new(app, "Kubernetes", nameGen, networking, resourceGroup);
            StorageAccounts storageAccounts = new(app, "Storage", nameGen, networking, resourceGroup);
            app.Synth();
        }
    }
}
