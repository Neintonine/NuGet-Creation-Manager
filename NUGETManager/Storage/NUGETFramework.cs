using System;

namespace NUGETManager.Storage
{
    [Serializable]
    public class NUGETFramework
    {
        public static NUGETFramework Any = new NUGETFramework()
        {
            _isAny = true,
            Name = "<Any>",
            DependencyIdentifier = "any",
            ReferenceIdentifier = "any"
        };

        internal bool _isAny = false;

        public string Name { get; set; }

        public string ReferenceIdentifier { get; set; }
        public string DependencyIdentifier { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}