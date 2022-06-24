using Constructs;
using HashiCorp.Cdktf;
using VFBlazor6._0.Utility;
using HashiCorp.Cdktf.Providers.Azurerm;

namespace VFBlazor6._0.TerraformLogic
{
    internal class StorageAccounts : TerraformStack
    {
        internal readonly StorageAccount _storageAccount;
        internal readonly StorageShare _storageShare;
        /// <summary>
        /// The StorageAccounts class contains all the definitions of the Azure resources which are directly related to StorageAccounts
        /// </summary>
        /// <param name="scope">an object of class Construct</param>
        /// <param name="id">a string identifier</param>
        /// <param name="ng">an object of class NameGenerator</param>
        /// <param name="nw">an object of class Networking</param>
        /// <param name="rg">an object of class ResourceGroups</param>
        internal StorageAccounts(Construct scope, string id, NameGenerator ng, Networking nw, ResourceGroups rg  ) : base(scope, id)
        {
            string[] _ips = new string[] { "111.111.111.000", "111.111.111.001" };
            
            AzurermProvider azurermProvider = new(this, "AzureRm", new AzurermProviderConfig
            {
                Features = new AzurermProviderFeatures(),
            });

            StorageAccount storageAccount = new StorageAccount(this, "azurerm_storage_account", new StorageAccountConfig
            {
                Name = ng.GetResNames()["Storage"],
                ResourceGroupName = ng.GetResNames()["RgName"],
                Location = ng._region[1],

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

            StorageShare storageShare = new StorageShare(this, "azurerm_storage_share", new StorageShareConfig
            {
                Name = "authtickets",
                StorageAccountName = storageAccount.Name,
                Quota = 5
            });

            _storageAccount = storageAccount;
            _storageShare = storageShare;
        }
    }
}
