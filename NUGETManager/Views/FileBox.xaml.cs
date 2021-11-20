using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace NUGETManager.Views
{
    /// <summary>
    /// Interaction logic for FileBox.xaml
    /// </summary>
    public partial class FileBox : UserControl
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.
            Register("Path", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        
        public static readonly DependencyProperty FileFilterProperty = DependencyProperty.
            Register("FileFilter", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(""));

        public static readonly DependencyProperty FolderProperty = DependencyProperty.
            Register("Folder", typeof(bool), typeof(FileBox), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty StartProperty = DependencyProperty.
            Register("StartPath", typeof(string), typeof(FileBox), new FrameworkPropertyMetadata(""));

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        public string FileFilter
        {
            get => (string)GetValue(FileFilterProperty);
            set => SetValue(FileFilterProperty, value);
        }
        public bool Folder
        {
            get => (bool)GetValue(FolderProperty);
            set => SetValue(FolderProperty, value);
        }

        public string StartPath
        {
            get => (string) GetValue(StartProperty);
            set => SetValue(PathProperty, value);
        }

        public FileBox()
        {
            InitializeComponent();
            RootPanel.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Folder)
            {

                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = FileFilter,
                    Multiselect = false,
                    InitialDirectory = StartPath
                };

                if (ofd.ShowDialog() == false) return;

                Path = ofd.FileName;
            }
            else
            {
                using (var fbd = new FolderBrowserDialog() {SelectedPath = StartPath})
                {
                    if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        Path = fbd.SelectedPath+"\\";
                    }
                }
            }
        }
    }
}
