using System;
using System.IO;

namespace NUGETManager.Storage
{
    [Serializable]
    public enum EventType
    {
        BeforeBuilding,
        AfterBuilding,
        BeforeUpload,
        AfterUpload,
    }

    [Serializable]
    public class EventObject
    {
        internal Project _connectedProject;

        private string _executionPath = ".";

        public EventType EventType { get; set; }

        public string ExecutionPath
        {
            get => _executionPath;
            set
            {
                Uri uri1 = new Uri(Path.GetFullPath(value), UriKind.Absolute);
                Uri uri2 = new Uri(Path.GetFullPath(_connectedProject.ProjectPath), UriKind.Absolute);
                _executionPath = uri2.MakeRelativeUri(uri1).OriginalString;
            }
        }

        public string BatchCode { get; set; }

        public string CreateBatch()
        {
            return $"set _prevCD=%cd%\n" +
                   $"cd \"{_executionPath}\"\n" +
                    BatchCode +
                   $"\ncd \"%_prevCD%\"";
        }
    }
}