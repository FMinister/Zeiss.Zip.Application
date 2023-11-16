using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using Zeiss.Zip.Core.Models;
using Zeiss.Zip.Core.Services;

namespace Zeiss.Zip.Core.ViewModels
{
    public class ZipFileExplorerViewModel : MvxViewModel
    {
        private ObservableCollection<FileModel> _file = new();
        private int _progressPercentage;

        public ZipFileExplorerViewModel()
        {
            ResetFileListCommand = new MvxCommand(ResetFileList);
        }

        public bool CanCreateZipFile => File?.Count > 0;
        public bool CanResetFiles => File?.Count > 0;

        public IMvxCommand ResetFileListCommand { get; set; }

        public ObservableCollection<FileModel> File
        {
            get => _file;
            set
            {
                _ = SetProperty(ref _file, value);
                _ = RaisePropertyChanged(() => CanCreateZipFile);
                _ = RaisePropertyChanged(() => CanResetFiles);
            }
        }

        public async Task CreateZipFile(string outputZipPath)
        {
            if (_file.Count > 0)
            {
                Progress<ProgressReportModel> progresss = new();
                progresss.ProgressChanged += ReportProgress;

                await Task.Run(() => MergeArchivesService.MergeArchives(File, progresss, outputZipPath));
            }
        }

        public void ResetFileList()
        {
            File.Clear();
            _ = RaiseAllPropertiesChanged();
        }

        public void HandleAddFiles(string[] filePath)
        {
            foreach (string file in filePath)
            {
                FileModel fileModel = new()
                {
                    FileName = Path.GetFileName(file),
                    Path = file
                };
                File.Add(fileModel);
            }
            _ = RaiseAllPropertiesChanged();
        }

        public int ProgressPercentage
        {
            get => _progressPercentage;
            set => _ = SetProperty(ref _progressPercentage, value);
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            ProgressPercentage = e.PercentageComplete;
        }
    }
}
