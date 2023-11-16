using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using Zeiss.Zip.Core.Models;

namespace Zeiss.Zip.Core.Services
{
    public class MergeArchivesService
    {
        public static void MergeArchives(ObservableCollection<FileModel> files, IProgress<ProgressReportModel> progress, string outputZipPath)
        {
            try
            {
                ProgressReportModel report = new();
                using FileStream zipToCreate = new(outputZipPath, FileMode.Create);
                using ZipArchive archive = new(zipToCreate, ZipArchiveMode.Create);

                for (int i = 0; i < files.Count; i++)
                {
                    using ZipArchive sourceArchive = ZipFile.OpenRead(files[i].Path);
                    foreach (ZipArchiveEntry entry in sourceArchive.Entries)
                    {
                        ZipArchiveEntry newEntry = archive.CreateEntry(entry.FullName);

                        using Stream entryStream = entry.Open();
                        using Stream newEntryStream = newEntry.Open();
                        entryStream.CopyTo(newEntryStream);
                    }

                    report.PercentageComplete = i * 100 / files.Count;
                    progress.Report(report);
                }

                report.PercentageComplete = 100;
                progress.Report(report);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error merging archives: {ex.Message}");
            }
        }
    }
}
