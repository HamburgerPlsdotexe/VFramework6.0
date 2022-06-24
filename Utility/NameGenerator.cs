using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VFBlazor6._0.Utility
{
    /// <summary>
    /// The NameGenerator class is passed to multiple resource classes of Terraform
    /// in order to adhere by the naming conventions of operations.
    /// The result of the class is simply an object, which usually calls GetResNames() 
    /// which in turn, returns a dictionary containing all the names of the resources.
    /// </summary>
    internal class NameGenerator
    {
        static int counter;

        internal readonly string _customer;
        internal readonly string _solution;
        internal readonly string[] _region;
        internal readonly string _envKind;

        internal readonly string _subnetRole1;
        internal readonly string _subnetRole2;

        internal readonly string _vmOs1;
        internal readonly string _vmOs2;

        internal readonly string _database1;
        internal readonly string _database2;

        private readonly Dictionary<string, string> _resourceNames = new Dictionary<string, string>();

        internal NameGenerator(string Customer, string Solution, string[] Region, string EnvKind="devtest", 
            string VMOs1= "w", string VMOs2="l", string SubnetRole1="web", string SubnetRole2="sql", string Database1="auth", string Database2="backlog")
        {
            _customer = Customer;
            _solution = Solution;
            _region = Region;
            _envKind = EnvKind;
            _vmOs1 = VMOs1;
            _vmOs2 = VMOs2;
            _subnetRole1 = SubnetRole1;
            _subnetRole2 = SubnetRole2;
            _database1 = Database1;
            _database2 = Database2;
            Console.WriteLine("Solution: " + Solution);
            Console.WriteLine("Region: " + Region[1]);
            Console.WriteLine("Abbreviation: " + RegionAbbreviation(Region[0]));
            Console.WriteLine(EnvironmentName("long")); 
            FillDict();
        }

        /// <summary>
        /// Getter of the dictionary containing all the names of the resources
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetResNames()
        {
            return _resourceNames;
        }

        /// <summary>
        /// The EnvironmentName method creates the intial part 
        /// of a resource's entire name 
        /// based on a few parameters
        /// </summary>
        /// <param name="variant"> can be either long or short </param>
        /// <param name="role"> can be either web or sql</param>
        /// <param name="os"> can be either l or w</param>
        /// <param name="env"> can be either prod or env or empty </param>
        /// <returns>E.g. "cassys-we" </returns>
        internal string EnvironmentName(string variant, string role = "", string os = "", string env = "")
        {
            switch ((variant, role, env))
            {
                case ("long", "", ""):
                    return _customer.ToLower() + _solution.ToLower() + "-" + RegionAbbreviation(_region[0]);    //E.g. cassys-we

                case ("short", "", ""):
                    return _customer.ToLower() + _solution.ToLower() + RegionAbbreviation(_region[0]) + _envKind;//E.g. cassys 
                        
                case ("VM", "web", "") or ("VM", "sql", ""):
                    return EnvironmentName("short", "") + RegionAbbreviation(_region[0]) + role + os;           //E.g. cassyswebwp 

                case ("long", "", "env"):
                    return _customer.ToLower() + "-" + _solution.ToLower() + "-" + _envKind;                   //E.g. cas-sys-devtest

                default:
                    return "";
            }
        }

        /// <summary>
        /// Creates an abbreviation which is the product of an 
        /// Azure region's entire name. This region name
        /// is obtained through the index, input by the user.
        /// </summary>
        /// <param name="region"> </param>
        /// <returns>E.g. "we" for West Europe </returns>
        private string RegionAbbreviation(string region)
        {
            return String.Join(String.Empty, region.Split(new[] { ' ' }).Select(word => word.First())).ToLower();
        }
        /// <summary>
        /// Fills the dictionary which will contain all of the resource names for an environment.
        /// Based on the naming conventions of operations.
        /// </summary>
        private void FillDict()
        {
            _resourceNames.Add("RgName", EnvironmentName("long") + "-" + _envKind + "-rg");
            _resourceNames.Add("VNetName", EnvironmentName("long") + "-" + _envKind + "-vnet");
            _resourceNames.Add("VNetSubnet1", EnvironmentName("long") + "-" + _envKind + "-" + _subnetRole1 + "-subnet");
            _resourceNames.Add("NSG1", _resourceNames["VNetSubnet1"] + "-nsg");
            _resourceNames.Add("VNetSubnet2", EnvironmentName("long") + "-" + _envKind + "-" + _subnetRole2 + "-subnet");
            _resourceNames.Add("NSG2", _resourceNames["VNetSubnet2"] + "-nsg");
            _resourceNames.Add("Monitoring", EnvironmentName("short") + "-" + _envKind + "-log");
            _resourceNames.Add("KeyVault", EnvironmentName("short").ToUpper() + "-" + _envKind + "-KeyVault");
            _resourceNames.Add("Automation", EnvironmentName("short").ToUpper() + "-" + _envKind + "-Automation");
            _resourceNames.Add("Integration", EnvironmentName("short").ToUpper() + "-" + _envKind + "-Integration");
            _resourceNames.Add("Storage", EnvironmentName("short")  + "storage");
            _resourceNames.Add("VM1AS", EnvironmentName("VM", _subnetRole1, _vmOs1) + "-as");
            _resourceNames.Add("VM1", EnvironmentName("VM", _subnetRole1, _vmOs1) + _envKind[0] + counter.ToString("D2"));
            _resourceNames.Add("VM1NIC", EnvironmentName("VM", _subnetRole1, _vmOs1) + _envKind[0] + counter.ToString("D2") + "-nic-" + counter.ToString("D2"));
            _resourceNames.Add("VM1NSG", EnvironmentName("VM", _subnetRole1, _vmOs1) + _envKind[0] + counter.ToString("D2") + "-nsg");
            _resourceNames.Add("VM2AS", EnvironmentName("VM", _subnetRole2, _vmOs2) + "-as");
            _resourceNames.Add("VM2", EnvironmentName("VM", _subnetRole2, _vmOs2) + _envKind[0] + counter.ToString("D2"));
            _resourceNames.Add("VM2NIC", EnvironmentName("VM", _subnetRole2, _vmOs2) + _envKind[0] + counter.ToString("D2") + "-nic-" + counter.ToString("D2"));
            _resourceNames.Add("VM2NSG", EnvironmentName("VM", _subnetRole2, _vmOs2) + _envKind[0] + counter.ToString("D2") + "-nsg");
            _resourceNames.Add("OSDisk", _resourceNames["VM1"] + "-os-disk");
            _resourceNames.Add("DataDisk", _resourceNames["VM1"] + "-data-disk-" + counter.ToString("D2"));
            _resourceNames.Add("Elastic", EnvironmentName("long") + "-" + _envKind + "-" + _subnetRole2 + "-pool");
            _resourceNames.Add("SQLServName", EnvironmentName("long") + "-" + _envKind + "-" + _subnetRole2 + "-server-" + counter.ToString("D2"));
            _resourceNames.Add("DTBName1", EnvironmentName("short").ToUpper() + "-" + _envKind + "-" + _database1);
            _resourceNames.Add("DTBName2", EnvironmentName("short").ToUpper() + "-" + _envKind + "-" + _database2);
            _resourceNames.Add("K8sName", EnvironmentName("long") + "-aks");
            counter++;
        }
    }
}
