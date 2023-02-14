using System;
using System.Collections.Generic;
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

namespace _3mfSantizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileSelect(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".3mf";
            dlg.Filter = "3MF files (*.3mf)|*.3mf";
            if (true == dlg.ShowDialog())
            {
                int configsRemoved = ConfigSanitizer.SanitizeInPlace(dlg.FileName);
                string completeMessage = string.Format("Sanitation Complete! {0} config files removed.", configsRemoved);
                MessageBox.Show(completeMessage, "Sanitation Complete!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void FolderSelect(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    int filesSanitized = 0;

                    foreach (string file in Directory.GetFiles(dialog.SelectedPath,"*3mf",SearchOption.AllDirectories))
                    {
                        int configsRemoved = ConfigSanitizer.SanitizeInPlace(file);
                        if (configsRemoved > 0) filesSanitized += 1;
                    }

                    string completeMessage = string.Format("Sanitation Complete! {0} 3mf files were sanitized.", filesSanitized);
                    MessageBox.Show(completeMessage, "Sanitation Complete!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
