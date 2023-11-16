using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using System;
using Zeiss.Zip.Core.ViewModels;

namespace Zeiss.Zip.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ZipFileExplorerView.xaml
    /// </summary>
    public partial class ZipFileExplorerView : MvxWpfView
    {
        public ZipFileExplorerView()
        {
            InitializeComponent();
        }

        public void OnOpenSelectFileDialog(object sender, EventArgs e)
        {
            ZipFileExplorerViewModel? viewModel = DataContext as ZipFileExplorerViewModel;
            OpenFileDialog openFileDialog = new()
            {
                DefaultExt = ".zip",
                Filter = "Archivdateien|*.zip|Alle Dateien|*.*",
                Multiselect = true,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.HandleAddFiles(openFileDialog.FileNames);
            }
        }

        public async void OnSaveSelectDialog(object sender, EventArgs e)
        {
            ZipFileExplorerViewModel? viewModel = DataContext as ZipFileExplorerViewModel;
            SaveFileDialog opdenSaveFileDialog = new()
            {
                DefaultExt = ".zip",
                FileName = "Output",
                Filter = "Archivdateien|*.zip|Alle Dateien|*.*",
            };

            if (opdenSaveFileDialog.ShowDialog() == true)
            {
                await viewModel.CreateZipFile(opdenSaveFileDialog.FileName);
            }
        }
    }
}
