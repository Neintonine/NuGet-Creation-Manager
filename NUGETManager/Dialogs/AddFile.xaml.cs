using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NUGETManager.Annotations;
using NUGETManager.Storage;
using Path = System.IO.Path;

namespace NUGETManager.Dialogs
{
    /// <summary>
    /// Interaction logic for AddFile.xaml
    /// </summary>
    public partial class AddFile : Window
    {
        private Project _proj;

        public NUGETFile File;

        public AddFile(Project project, [CanBeNull] string fixedStoreLocation = null, [CanBeNull] NUGETFile fileToChange = null)
        {
            _proj = project;

            InitializeComponent();
            DataContext = File = fileToChange ?? new NUGETFile()
            {
                RelativePath = true
            };

            if (!string.IsNullOrEmpty(fixedStoreLocation))
            {
                StoreLocation.Text = fixedStoreLocation;
                StoreLocation.IsEnabled = false;
            }

            if (fileToChange != null)
            {
                Uri path = new Uri(new Uri(project.ProjectPath), fileToChange.Path);

                FileBox.Path = path.LocalPath;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (File.RelativePath)
            {
                Uri uri1 = new Uri(Path.GetFullPath(FileBox.Path), UriKind.Absolute);
                Uri uri2 = new Uri(Path.GetFullPath(_proj.ProjectPath), UriKind.Absolute);
                File.Path = uri2.MakeRelativeUri(uri1).OriginalString;
            }
            else
            {
                File.Path = FileBox.Path;
            }

            File.StoreLocation = StoreLocation.Text;

            DialogResult = true;
        }
    }
}
