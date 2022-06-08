using System;
using System.IO;
using Newtonsoft.Json;

namespace VFBlazor6._0.Utility
{
    static class FileOps
    {
        internal static string _solutionDir = Directory.GetCurrentDirectory();

        internal static void CleanUpOutFolder()
        {
            try
            {
                string path = _solutionDir + @"\cdktf.out\stacks\azure\cdk.tf.json";
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static Dictionary<string, string> JsonFileToDict (string fileLoc)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(fileLoc));
            }

            catch(Exception e)
            {
                return new Dictionary<string, string>() { { "error" , e.ToString() } };
            }
        }
    }
}
