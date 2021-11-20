using System.Collections.Generic;
using System.IO;

namespace NUGETManager.Storage
{
    public class BatchCode
    {
        public static Dictionary<string, string> BatchVariables = new Dictionary<string, string>()
        {
            { "executionPath", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) },

        };

        public static readonly string NugetPackage = "echo # Packing NUGET Package\n%executionPath%\\nuget.exe pack %nuspecPath%\n";

        public static string GetVariables()
        {
            string result = "";

            foreach (KeyValuePair<string, string> pair in BatchVariables)
            {
                result += $"set {pair.Key}={pair.Value}\n";
            }

            return result;
        }
    }
}