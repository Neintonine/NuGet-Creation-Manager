using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrWPF.Windows.Data;
using NUGETManager.Dialogs;
using NUGETManager.Storage;

namespace NUGETManager.Bindings
{
    public class ProjectBinding : BindingBase
    {
        private ICommand _addFrameworkCommand;

        private ICommand _addVersionCommand;

        private ICommand _addReferenceCommand;
        private ICommand _editReferenceCommand;
        private ICommand _removeReferenceCommand;

        private ICommand _addDependencyCommand;
        private ICommand _editDependencyCommand;
        private ICommand _removeDependencyCommand;

        private ICommand _addFileCommand;
        private ICommand _editFileCommand;
        private ICommand _removeFileCommand;

        private ICommand _generateNuspecCommand;

        private Project _project;

        internal NUGETFramework currentFramework;

        public string Title
        {
            get => _project.Title;
            set
            {
                if (_project.Title != value)
                {
                    _project.Title = value;
                    OnPropertyChanged();

                    MainWindow.Window.FileExplorer.DataContext = null;
                    MainWindow.Window.FileExplorer.DataContext = MainListBinding.Binding;
                    MainWindow.Window.FileBox.SelectedItem = _project;
                }
            }
        }

        public string Description
        {
            get => _project.Description;
            set
            {
                if (_project.Description != value)
                {
                    _project.Description = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Authors
        {
            get => _project.Authors;
            set
            {
                if (_project.Authors != value)
                {
                    _project.Authors = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Tags
        {
            get => _project.Tags;
            set
            {
                if (_project.Tags != value)
                {
                    _project.Tags = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ProjectURL
        {
            get => _project.ProjectURL;
            set
            {
                if (_project.ProjectURL != value)
                {
                    _project.ProjectURL = value;
                    OnPropertyChanged();
                }
            }
        }

        public string License
        {
            get => _project.License;
            set
            {
                if (_project.License != value)
                {
                    _project.License = value;
                    OnPropertyChanged();
                }
            }
        }

        public string APIKey
        {
            get => _project.APIKey;
            set
            {
                if (_project.APIKey != value)
                {
                    _project.APIKey = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Identifier
        {
            get => _project.Identifier;
            set
            {
                if (_project.Identifier != value)
                {
                    _project.Identifier = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool RequiresAcceptance
        {
            get => _project.RequireLicenseAcceptance;
            set
            {
                if (_project.RequireLicenseAcceptance != value)
                {
                    _project.RequireLicenseAcceptance = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Copyright
        {
            get => _project.Copyright;
            set
            {
                if (_project.Copyright != value)
                {
                    _project.Copyright = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ProjectPath
        {
            get => _project.ProjectPath;
            set
            {
                if (_project.ProjectPath != value)
                {
                    _project.ProjectPath = value;
                    OnPropertyChanged();
                }
            }
        }
        public Brush ProjectPathBorder
        {
            get => File.Exists(_project.ProjectPath) ? Brushes.Red : Brushes.Green;
        }

        public string TargetLocation
        {
            get => _project.TargetLocation;
            set => _project.TargetLocation = value;
        }

        public string CompilePath
        {
            get => _project.CompileFilePath;
            set
            {
                Uri uri1 = new Uri(Path.GetFullPath(value), UriKind.Absolute);
                Uri uri2 = new Uri(Path.GetFullPath(ProjectPath), UriKind.Absolute);
                _project.CompileFilePath = uri2.MakeRelativeUri(uri1).OriginalString;

                OnPropertyChanged();
            }
        }
        public bool Compile
        {
            get => _project.CompileFile;
            set
            {
                _project.CompileFile = value;
                OnPropertyChanged();
            }
        }

        public bool AutoDeletePackage
        {
            get => _project.DeletePackageAfterUpload;
            set => _project.DeletePackageAfterUpload = value;

        }

        public string IconPath
        {
            get => _project.Icon != null ? _project.Icon.Path : "";
            set
            {
                if (_project.Icon == null)
                {
                    _project.Icon = new NUGETFile()
                    {
                        RelativePath = true,
                        StoreLocation = "images"
                    };
                    _project.Files.Add(_project.Icon);
                }

                Uri uri1 = new Uri(Path.GetFullPath(value), UriKind.Absolute);
                Uri uri2 = new Uri(Path.GetFullPath(ProjectPath), UriKind.Absolute);
                _project.Icon.Path = uri2.MakeRelativeUri(uri1).OriginalString;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<NUGETFile> Files => _project.Files;

        public ObservableDictionary<NUGETFramework,
            Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>> References =>
            _project.References;

        public ObservableCollection<Storage.Version> Versions => _project.Versions;

        public ICommand AddFrameworkCommand =>
            _addFrameworkCommand ?? (_addFrameworkCommand = (CommandHandler) AddFramework);

        public ICommand AddReferenceCommand =>
            _addReferenceCommand ?? (_addReferenceCommand = (CommandHandler) AddReference);
        public ICommand EditReferenceCommand =>
            _editReferenceCommand ?? (_editReferenceCommand = (CommandHandler) EditReference);
        public ICommand RemoveReferenceCommand =>
            _removeReferenceCommand ?? (_removeReferenceCommand = (CommandHandler)RemoveReference);

        public ICommand AddFileCommand =>
            _addFileCommand ?? (_addFileCommand = (CommandHandler) AddFile);
        public ICommand EditFileCommand =>
            _editFileCommand ?? (_editFileCommand = (CommandHandler) EditFile);
        public ICommand RemoveFileCommand =>
            _removeFileCommand ?? (_removeFileCommand = (CommandHandler)RemoveFile);

        public ICommand AddDependencyCommand =>
            _addDependencyCommand ?? (_addDependencyCommand = (CommandHandler)AddDependency);
        public ICommand EditDependencyCommand =>
            _editDependencyCommand ?? (_editDependencyCommand = (CommandHandler)EditDependency);
        public ICommand RemoveDependencyCommand =>
            _removeDependencyCommand ?? (_removeDependencyCommand = (CommandHandler)RemoveDependency);

        public ICommand AddVersionCommand =>
            _addVersionCommand ?? (_addVersionCommand = (CommandHandler)AddVersion);

        public ICommand GenerateNUSPECCommand =>
            _generateNuspecCommand ?? (_generateNuspecCommand = (CommandHandler) PreviewNuspec);

        public ICommand PackNugetCommand => (CommandHandler) CreateNuget;
        public ICommand UploadNugetCommand => (CommandHandler) UploadPackage;

        public ProjectBinding(Project project)
        {
            _project = project;
        }

        private void AddFramework()
        {
            Dialogs.AddFramework dlg = new AddFramework();
            if (dlg.ShowDialog() == true)
            {
                if (!_project.References.ContainsKey(dlg.SelectedFramework))
                {
                    _project.References.Add(dlg.SelectedFramework,
                        new Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>(new ObservableCollection<NUGETFile>(), new ObservableCollection<NUGETDependency>()));
                }
            }
        }

        private void AddReference()
        {
            if (currentFramework == null) return;
            Dialogs.AddFile dlg = new AddFile(_project, "lib\\"+currentFramework.ReferenceIdentifier);
            if (dlg.ShowDialog() == true)
            {
                References[currentFramework].Item1.Add(dlg.File);
                Files.Add(dlg.File);
            }
        }
        private void EditReference()
        {
            if (currentFramework == null || MainWindow.Window.SelectedReferenceFile == null) return;
            Dialogs.AddFile dlg = new AddFile(_project, "lib\\" + currentFramework.ReferenceIdentifier, MainWindow.Window.SelectedReferenceFile);
            if (dlg.ShowDialog() == true)
            {
                MainWindow.Window.DataGrid_FileReferences.ItemsSource = null;
                MainWindow.Window.DataGrid_FileReferences.ItemsSource = References[currentFramework].Item1;
            }
        }
        private void RemoveReference()
        {
            if (currentFramework == null || MainWindow.Window.SelectedReferenceFile == null) return;

            References[currentFramework].Item1.Remove(MainWindow.Window.SelectedReferenceFile);
            Files.Remove(MainWindow.Window.SelectedReferenceFile);
            MainWindow.Window.DataGrid_FileReferences.ItemsSource = null;
            MainWindow.Window.DataGrid_FileReferences.ItemsSource = References[currentFramework].Item1;
        }

        private void AddFile()
        {
            Dialogs.AddFile dlg = new AddFile(_project);
            if (dlg.ShowDialog() == true)
            {
                Files.Add(dlg.File);
            }
        }
        private void EditFile()
        {
            Dialogs.AddFile dlg = new AddFile(_project, null, (NUGETFile)MainWindow.Window.DataGrid_Files.SelectedItem);
            if (dlg.ShowDialog() == true)
            {
                MainWindow.Window.DataGrid_Files.ItemsSource = null;
                MainWindow.Window.DataGrid_Files.ItemsSource = Files;
            }
        }
        private void RemoveFile()
        {
            Files.Remove((NUGETFile)MainWindow.Window.DataGrid_Files.SelectedItem);

            MainWindow.Window.DataGrid_Files.ItemsSource = null;
            MainWindow.Window.DataGrid_Files.ItemsSource = Files;
        }

        private void AddVersion()
        {
            _project.Versions.Insert(0, new Storage.Version());
            MainWindow.Window.DataGrid_Versions.SelectedIndex = 0;
        }

        private void AddDependency()
        {
            if (currentFramework == null) return;
            NUGETDependency dependency = new NUGETDependency();
            Dialogs.AddDependency dlg = new AddDependency(dependency);
            if (dlg.ShowDialog() == true)
            {
                References[currentFramework].Item2.Add(dependency);
            }
        }
        private void EditDependency()
        {
            if (currentFramework == null || MainWindow.Window.SelectedDependency == null) return;
            Dialogs.AddDependency dlg = new AddDependency(MainWindow.Window.SelectedDependency);
            dlg.ShowDialog();

            MainWindow.Window.DataGrid_Depenencies.DataContext = null;
            MainWindow.Window.DataGrid_Depenencies.DataContext = References[currentFramework].Item2;
        }
        private void RemoveDependency()
        {
            if (currentFramework == null || MainWindow.Window.SelectedDependency == null) return;
            References[currentFramework].Item2.Remove(MainWindow.Window.SelectedDependency);

            MainWindow.Window.DataGrid_Depenencies.DataContext = null;
            MainWindow.Window.DataGrid_Depenencies.DataContext = References[currentFramework].Item2;
        }

        private void PreviewNuspec()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".nuspec");
            
            _project.CreateNuspec().Save(path);
            Process.Start(path);
        }

        private void CreateNuget()
        {
            CreateNugetIntern();
        }

        private void UploadPackage()
        {
            string upload =
                $"%executionPath%\\nuget.exe push {Identifier}.{Versions[_project.FilledVersionIndex].Name}.nupkg {APIKey} -src https://api.nuget.org/v3/index.json \n";

            if (AutoDeletePackage)
            {
                upload += $"del {Identifier}.{Versions[_project.FilledVersionIndex].Name}.nupkg\n";
            }

            CreateNugetIntern(upload);
        }

        private void CreateNugetIntern(string uploadCode = null)
        {
            _project.Save();

            string path = Path.Combine(_project.ProjectPath, _project.Identifier + ".nuspec");

            _project.CreateNuspec().Save(path);

            if (!UserSettings.Settings.SawCMDNotification)
            {
                MessageBox.Show(
                    "Packing and uploading will show a command line-window.\nThat is made to ensure, events are properly executed and to allow custom massages to be shown correctly.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                UserSettings.Settings.SawCMDNotification = true;
                UserSettings.Save();
            }

            string batch = $"@echo off\ncd {_project.ProjectPath}\n";
            batch += $"set nuspecPath={path}\n";
            batch += BatchCode.GetVariables();

            if (Compile && !string.IsNullOrEmpty(CompilePath))
            {
                batch +=
                    $"echo # Building C#-project \n" +
                    $"\"{UserSettings.Settings.MSBuildExecutable}\" \"{CompilePath}\" -t:Clean;Build -clp:PerformanceSummary;Summary;ErrorsOnly\n" +
                    $"if not %errorlevel% == 0 (\n" +
                    $"  echo \"!!! BUILD FAILED !!!\"\n " +
                    $"  pause\n" +
                    $"  exit\n" +
                    $")\n";
            }

            batch += BatchCode.NugetPackage;
            if (uploadCode != null) batch += uploadCode;
            batch += "pause";
            File.WriteAllText(Path.Combine(_project.ProjectPath, "pack.bat"), batch);

            Process cmd = Process.Start(Path.Combine(_project.ProjectPath, "pack.bat"));
            cmd.WaitForExit();

            int version = _project.FilledVersionIndex;

            if (Directory.Exists(TargetLocation))
            {
                string start = Path.Combine(_project.ProjectPath, $"{Identifier}.{Versions[version].Name}.nupkg");

                if (File.Exists(start))
                {
                    string target = Path.Combine(TargetLocation, $"{Identifier}.{Versions[version].Name}.nupkg");
                    File.Copy(start, target, true);
                    File.Delete(start);
                }
            }
            else
            {
                MessageBox.Show("The target folder wasn't found. The created package is now in the project folder.");
            }

            File.Delete(Path.Combine(_project.ProjectPath, "pack.bat"));
        }
    }
}