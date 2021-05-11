using System;

namespace NUGETManager.Storage
{
    [Serializable]
    public class Version
    {
        public string Name { get; set; }
        public string ReleaseNotes { get; set; }
    }
}