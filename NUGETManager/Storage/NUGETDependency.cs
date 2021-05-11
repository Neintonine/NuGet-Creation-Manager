using System;

namespace NUGETManager.Storage
{
    [Serializable]
    public class NUGETDependency
    {
        public string Package { get; set; }
        public string Version { get; set; }
    }
}