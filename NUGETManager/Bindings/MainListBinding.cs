using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using NUGETManager.Dialogs;
using NUGETManager.Storage;

namespace NUGETManager.Bindings
{
    public class MainListBinding : BindingBase
    {

        public static MainListBinding Binding = new MainListBinding();

        private ICommand _addCommand;
        private ICommand _saveCommand;
        private ICommand _deleteCommand;

        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        public ICommand AddCommand => _addCommand ?? (_addCommand = (CommandHandler)AddProject);
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = (CommandHandler)SaveProject);
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = (CommandHandler)DeleteProject);

        public void AddProject()
        {
            ProjectInit initDialog = new ProjectInit();
            if (initDialog.ShowDialog() != true) return;

            Project project = new Project()
            {
                Identifier = initDialog.Identifier,
                ProjectPath = initDialog.ProjectPath
            };

            Projects.Add(project);
            MainWindow.Window.FileBox.SelectedIndex = Projects.Count - 1;
            MainWindow.Window.TitleBox.Focus();
            MainWindow.Window.TitleBox.Select(0, project.Title.Length);
        }

        public void SaveProject()
        {
            ((Project) MainWindow.Window.FileBox.SelectedItem).Save();
        }

        public void DeleteProject()
        {

            Project proj = (Project) MainWindow.Window.FileBox.SelectedItem;


            if (MessageBox.Show(
                    $"This will delete all saved information about the nuget-package '{proj.Identifier}'. Are you sure?",
                    "Are you sure?", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) ==
                MessageBoxResult.Cancel) return; 
                
            Projects.Remove(proj);

            if (File.Exists(proj.ProjectFile))
            {
                File.Delete(proj.ProjectFile);
            }
        }
    }
}