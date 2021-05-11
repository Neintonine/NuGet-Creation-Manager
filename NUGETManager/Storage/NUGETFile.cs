using System;

namespace NUGETManager.Storage
{
    [Serializable]
    public class NUGETFile
    {
        public string Path { get; set; }
        public bool RelativePath { get; set; }

        public string StoreLocation { get; set; }
    }
}