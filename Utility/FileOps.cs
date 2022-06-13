using System.IO.Compression;
using Newtonsoft.Json;

namespace VFBlazor6._0.Utility
{
    static class FileOps
    {
        internal static string _solutionDir = Directory.GetCurrentDirectory();
        internal static string _jsonOldLoc = _solutionDir + @"\cdktf.out\stacks\azure\cdk.tf.json";
        internal static string _jsonNewLoc = _solutionDir + @"\Files\cdk.tf.json";
        internal static string _zipLoc = _solutionDir + @"\Out\EnvironmentFiles.zip ";

        internal static void CleanFiles()
        {
            try
            {
                File.Delete(_jsonNewLoc);
                File.Delete(_zipLoc);
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

        internal static long GetFileSize(string pathToFile)
        {
            long length = new FileInfo(pathToFile).Length;

            return length;
        }

        internal static byte[] getFileBytes(string pathToFile)
        {
            return File.ReadAllBytes(pathToFile);
        }

        internal static void ZipFiles()
        {
            ZipFile.CreateFromDirectory(@".\Files", _zipLoc);
        }

        internal static void MoveFile(string path, string toPath)
        {
            File.Move(path, toPath, true);
        }

        internal static Stream GetFileStream(string pathToFile)
        {
            byte[] binaryData = getFileBytes(pathToFile);
            MemoryStream fileStream = new MemoryStream(binaryData);

            return fileStream;
        }
    }
}
