
namespace VFBlazor6._0.Utility
{
    internal static class Debug
    {
        internal static void PrintDir()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }

        internal static void PrintDirTree()
        {
            foreach (var items in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                Console.WriteLine(items);
            }
        }
        internal static void PrintDictContents(Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                if(dictionary.Count == 0)
                {
                    Console.WriteLine("\nDict is empty\n");
                }
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
