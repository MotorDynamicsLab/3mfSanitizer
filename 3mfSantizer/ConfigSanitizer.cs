using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;


namespace _3mfSantizer
{
    /// <summary>
    /// Opens a 3MF file and removes any config files stored in the metadata
    /// </summary>
    public class ConfigSanitizer
    {
        /// <summary>
        /// Sanitizes the provided 3MF file in place (old file is overwritten)
        /// </summary>
        /// <param name="filePath">Path of the file to sanitize</param>
        /// <returns>Number of config items removed from 3MF archive</returns>
        public static int SanitizeInPlace(string filePath)
        {
            int numConfigsRemoved = 0;
            filePath = Path.GetFullPath(filePath);
            ZipArchive archive = ZipFile.Open(filePath, ZipArchiveMode.Update);
            List<ZipArchiveEntry> deletionList = new List<ZipArchiveEntry>();

            //Build a list of files to delete from the archive
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".config", StringComparison.OrdinalIgnoreCase) &&
                    entry.FullName.StartsWith("Metadata/") )
                {
                    deletionList.Add(entry);
                }
            }

            //Delete all the files in the list
            foreach (ZipArchiveEntry entry in deletionList)
            {
                entry.Delete();
                ++numConfigsRemoved;
            }

            archive.Dispose();
            return numConfigsRemoved;
        }
    }
}
