using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DrWPF.Windows.Data;
using NUGETManager.Bindings;
using NUGETManager.Storage;
using Path = System.IO.Path;

namespace NUGETManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window;

        public NUGETFile SelectedReferenceFile;
        public NUGETFile SelectedFile;
        public NUGETDependency SelectedDependency;

        public MainWindow()
        {
            Window = this;

            UserSettings.Load();

            List<Project> loadedProjects = new List<Project>();
            foreach (string file in Directory.EnumerateFiles(Path.Combine(SaveManager.AppDataPath, "Projects")))
            {
                Project proj = SaveManager.Load<Project>("Projects\\" + Path.GetFileNameWithoutExtension(file));
                if (proj.Events == null) proj.Events = new EventList();
                proj.ProjectFile = file;
                loadedProjects.Add(proj);
            }
            MainListBinding.Binding.Projects = new ObservableCollection<Project>(loadedProjects);

            InitializeComponent();
            FileExplorer.DataContext = MainListBinding.Binding;
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            foreach (Project project in MainListBinding.Binding.Projects)
            {
                project.Save();
            }
        }

        public void Reset()
        {
            ToggleAPI.IsChecked = false;
        }

        private void MainListEvent(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = (ListBox) sender;

            if (box.SelectedItem == null)
            {
                ProjectView.IsEnabled = false;
                return;
            }

            ProjectView.DataContext = new ProjectBinding((Project) box.SelectedItem);
            ProjectView.IsEnabled = true;
        }

        private void ToggleAPI_CheckEvent(object sender, RoutedEventArgs e)
        {
            APIEntry.Visibility = ToggleAPI.IsChecked == true ? Visibility.Visible : Visibility.Hidden;
        }

        private void FrameworkChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectBinding binding = (ProjectBinding) ProjectView.DataContext;
            if (FrameworkCombo.SelectedItem == null)
            {
                DataGrid_Depenencies.ItemsSource = DataGrid_FileReferences.ItemsSource = null;

                return;
            }
            var selecteditem = (KeyValuePair<NUGETFramework, Tuple<ObservableCollection<NUGETFile>, ObservableCollection<NUGETDependency>>>) FrameworkCombo.SelectedItem;

            binding.currentFramework = selecteditem.Key;
            DataGrid_FileReferences.ItemsSource = selecteditem.Value.Item1;
            DataGrid_Depenencies.ItemsSource = selecteditem.Value.Item2;
        }

        private void ReferenceChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedReferenceFile = (NUGETFile) DataGrid_FileReferences.SelectedItem;
        }

        private void DataGrid_Versions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Version_GRID.DataContext = DataGrid_Versions.SelectedItem;
        }

        private void DataGrid_Depenencies_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDependency = (NUGETDependency) DataGrid_Depenencies.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EventsGrid.ItemsSource == null) return;

            EventList list = (EventList) EventsGrid.ItemsSource;
            list.Add(new EventObject()
            {
                _connectedProject = (Project)FileBox.SelectedItem
            });
        }
    }
}
