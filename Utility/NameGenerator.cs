using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VFBlazor6._0.Utility
{
    internal class NameGenerator
    {
        static int counter;

        internal readonly string Customer;
        internal readonly string Solution;
        internal readonly string[] Region;
        internal readonly string EnvKind;

        internal readonly string SubnetRole1;
        internal readonly string SubnetRole2;

        internal readonly string VMOs1;
        internal readonly string VMOs2;

        internal readonly string Database1;
        internal readonly string Database2;

        private Dictionary<string, string> ResourceNames = new Dictionary<string, string>();

        internal NameGenerator(string Customer, string Solution, string[] Region, string EnvKind="devtest", string VMOs1= "w", string VMOs2="l", string SubnetRole1="web", string SubnetRole2="sql", string Database1="auth", string Database2="backlog")
        {
            this.Customer = Customer;
            this.Solution = Solution;
            this.Region = Region;
            this.EnvKind = EnvKind;
            this.VMOs1 = VMOs1;
            this.VMOs2 = VMOs2;
            this.SubnetRole1 = SubnetRole1;
            this.SubnetRole2 = SubnetRole2;
            this.Database1 = Database1;
            this.Database2 = Database2;
            Console.WriteLine("Solution: " + Solution);
            Console.WriteLine("Region: " + Region[1]);
            Console.WriteLine("Abbreviation: " + RegionAbbreviation(Region[0]));
            Console.WriteLine(EnvironmentName("long")); 
            FillDict();
        }

        public Dictionary<string, string> GetResNames()
        {
            return ResourceNames;
        }

        internal string EnvironmentName(string variant, string role = "", string os = "", string env = "")
        {
            switch ((variant, role, env))
            {
                case ("long", "", ""):
                    return Customer.ToLower() + Solution.ToLower() + "-" + RegionAbbreviation(Region[0]);    //E.g. cassys-we

                case ("short", "", ""):
                    return Customer.ToLower() + Solution.ToLower();                                         //E.g. cassys 
                        
                case ("VM", "web", "") or ("VM", "sql", ""):
                    return EnvironmentName("short", "") + RegionAbbreviation(Region[0]) + role + os;        //E.g. cassyswebwp 

                case ("long", "", "env"):
                    return Customer.ToLower() + "-" + Solution.ToLower() + "-" + EnvKind;                   //E.g. cas-sys-devtest

                default:
                    return "";
            }
        }

        private string RegionAbbreviation(string region)
        {
            return String.Join(String.Empty, region.Split(new[] { ' ' }).Select(word => word.First())).ToLower();
        }

        private void FillDict()
        {
            ResourceNames.Add("RgName", EnvironmentName("long") + "-" + EnvKind + "-rg");
            ResourceNames.Add("VNetName", EnvironmentName("long") + "-" + EnvKind + "-vnet");
            ResourceNames.Add("VNetSubnet1", EnvironmentName("long") + "-" + EnvKind + "-" + SubnetRole1 + "-subnet");
            ResourceNames.Add("NSG1", ResourceNames["VNetSubnet1"] + "-nsg");
            ResourceNames.Add("VNetSubnet2", EnvironmentName("long") + "-" + EnvKind + "-" + SubnetRole2 + "-subnet");
            ResourceNames.Add("NSG2", ResourceNames["VNetSubnet2"] + "-nsg");
            ResourceNames.Add("Monitoring", EnvironmentName("short") + "-" + EnvKind + "-log");
            ResourceNames.Add("KeyVault", EnvironmentName("short").ToUpper() + "-" + EnvKind + "-KeyVault");
            ResourceNames.Add("Automation", EnvironmentName("short").ToUpper() + "-" + EnvKind + "-Automation");
            ResourceNames.Add("Integration", EnvironmentName("short").ToUpper() + "-" + EnvKind + "-Integration");
            ResourceNames.Add("Storage", EnvironmentName("short") + Region + EnvKind + "storage");
            ResourceNames.Add("VM1AS", EnvironmentName("VM", SubnetRole1, VMOs1) + "-as");
            ResourceNames.Add("VM1", EnvironmentName("VM", SubnetRole1, VMOs1) + EnvKind[0] + counter.ToString("D2"));
            ResourceNames.Add("VM1NIC", EnvironmentName("VM", SubnetRole1, VMOs1) + EnvKind[0] + counter.ToString("D2") + "-nic-" + counter.ToString("D2"));
            ResourceNames.Add("VM1NSG", EnvironmentName("VM", SubnetRole1, VMOs1) + EnvKind[0] + counter.ToString("D2") + "-nsg");
            ResourceNames.Add("VM2AS", EnvironmentName("VM", SubnetRole2, VMOs2) + "-as");
            ResourceNames.Add("VM2", EnvironmentName("VM", SubnetRole2, VMOs2) + EnvKind[0] + counter.ToString("D2"));
            ResourceNames.Add("VM2NIC", EnvironmentName("VM", SubnetRole2, VMOs2) + EnvKind[0] + counter.ToString("D2") + "-nic-" + counter.ToString("D2"));
            ResourceNames.Add("VM2NSG", EnvironmentName("VM", SubnetRole2, VMOs2) + EnvKind[0] + counter.ToString("D2") + "-nsg");
            ResourceNames.Add("OSDisk", ResourceNames["VM1"] + "-os-disk");
            ResourceNames.Add("DataDisk", ResourceNames["VM1"] + "-data-disk-" + counter.ToString("D2"));
            ResourceNames.Add("Elastic", EnvironmentName("long") + "-" + EnvKind + "-" + SubnetRole2 + "-pool");
            ResourceNames.Add("SQLServName", EnvironmentName("long") + "-" + EnvKind + "-" + SubnetRole2 + "-server-" + counter.ToString("D2"));
            ResourceNames.Add("DTBName1", EnvironmentName("short").ToUpper() + "-" + EnvKind + "-" + Database1);
            ResourceNames.Add("DTBName2", EnvironmentName("short").ToUpper() + "-" + EnvKind + "-" + Database2);
            counter++;
        }
    }
}
