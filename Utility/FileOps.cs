using System.IO.Compression;
using Newtonsoft.Json;

namespace VFBlazor6._0.Utility
{
    internal static class FileOps
    {
        internal static string _solutionDir = Directory.GetCurrentDirectory();
        internal static string _batLoc = _solutionDir + @"\Files\AutomatedEnvironmentPlanner.bat";
        internal static string _jsonOldLoc = _solutionDir + @"\cdktf.out\stacks\azure\cdk.tf.json";
        internal static string _jsonNewLoc = _solutionDir + @"\Files\cdk.tf.json";
        internal static string _zipLoc = _solutionDir + @"\Out\EnvironmentFiles.zip";
        internal static string _multipleJson = _solutionDir + @"cdktf.out\stacks";
        /// <summary>
        /// Function which is responsible for cleaning the generated files
        /// </summary>
        internal static void CleanFiles()
        {
            try
            {
                File.Delete(_jsonNewLoc);
                File.Delete(_zipLoc);
                File.Delete(_multipleJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Function which is responsible for reading JSON files and creating dictionaries
        /// </summary>
        /// <param name="fileLoc">String location to the JSON file</param>
        /// <returns>A dictionary object in the form of string keys and string values</returns>
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
        /// <summary>
        /// Returns the file size of any given file which it is provided
        /// </summary>
        /// <param name="pathToFile">String location to the file </param>
        /// <returns> A long number with the size </returns>
        internal static long GetFileSize(string pathToFile)
        {
            long length = new FileInfo(pathToFile).Length;

            return length;
        }
        /// <summary>
        /// Returns a byte[] of the file which it is provided
        /// </summary>
        /// <param name="pathToFile">String location to the file</param>
        /// <returns> an byte[] </returns>
        internal static byte[] GetFileBytes(string pathToFile)
        {
            return File.ReadAllBytes(pathToFile);
        }
        /// <summary>
        /// Zips a folder and places it in a location
        /// </summary>
        /// <param name="directoryToZip">String path to the folder which should be zipped</param>
        /// <param name="newDirLoc">String path to the location where the zip should be stored </param>
        internal static void ZipFiles(string directoryToZip, string newDirLoc)
        {
            ZipFile.CreateFromDirectory(directoryToZip, newDirLoc);
        }
        /// <summary>
        /// Moves any given file
        /// </summary>
        /// <param name="pathToFile">String path to the location of the file which should be moved, together with it's original name</param>
        /// <param name="toNewPath">String path to the location where the file should be moved to, together with it's original name</param>
        internal static void MoveFile(string pathToFile, string toNewPath)
        {
            File.Move(pathToFile, toNewPath, true);
        }
        /// <summary>
        /// Creates a filestream which is used to facilitate a way of downloading a folder to a clients machine. Used together with JavaScript
        /// </summary>
        /// <param name="pathToFile">String path to the file</param>
        /// <returns>A Stream object containing the stream of the file</returns>
        internal static Stream GetFileStream(string pathToFile)
        {
            byte[] binaryData = GetFileBytes(pathToFile);
            MemoryStream fileStream = new MemoryStream(binaryData);

            return fileStream;
        }
    }
}
