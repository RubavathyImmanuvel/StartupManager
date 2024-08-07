using Microsoft.Win32;
using System.Windows;

namespace StartupManager
{
    public partial class MainWindow : Window
    {
        private const string StartupRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public MainWindow()
        {
            InitializeComponent();
            LoadStartupPrograms();
        }

        private void LoadStartupPrograms()
        {
            StartupProgramsList.Items.Clear();
            using (var key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
            {
                if (key != null)
                {
                    foreach (var valueName in key.GetValueNames())
                    {
                        var value = key.GetValue(valueName);
                        StartupProgramsList.Items.Add($"{valueName}: {value}");
                    }
                }
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                var name = System.IO.Path.GetFileNameWithoutExtension(filePath);

                using (var key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
                {
                    key.SetValue(name, filePath);
                }

                LoadStartupPrograms();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartupProgramsList.SelectedItem != null)
            {
                var selectedItem = StartupProgramsList.SelectedItem.ToString();
                var name = selectedItem.Split(':')[0];

                using (var key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
                {
                    key.DeleteValue(name);
                }

                LoadStartupPrograms();
            }
        }
    }
}
