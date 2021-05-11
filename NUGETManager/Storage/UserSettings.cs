using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace NUGETManager.Storage
{
    [Serializable]
    public class UserSettings
    {
        public static UserSettings Settings;

        public ObservableCollection<NUGETFramework> StoredFrameworks = new ObservableCollection<NUGETFramework>();
        public bool SawCMDNotification = false;
        public string MSBuildExecutable = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe";

        public static void Save()
        {
            SaveManager.Save("settings", Settings);
        }

        public static void Load()
        {
            Settings = SaveManager.Load<UserSettings>("settings") ?? new UserSettings();
        }
    }
}