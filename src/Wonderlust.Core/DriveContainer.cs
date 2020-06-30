using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;
using SystemPath = System.IO.Path;

namespace Wonderlust.Core
{
    public class DriveContainer : IContainer
    {
        public string Path { get; }
        public string Name { get => SystemPath.GetFileName(Path); }

        public DateTime? DateTime { get; }

        public DriveContainer(string path, DateTime dateTime)
        {
            this.Path = path;
            DateTime = dateTime;
        }

        public IContainer? GetParent()
        {
            var dirName = SystemPath.GetDirectoryName(Path);
            if (dirName == null) return null;

            return new DriveContainer(dirName, Directory.GetLastWriteTime(dirName));            
        }

        public IEnumerable<IContainerItem> GetItems()
        {
            foreach (var child in Directory.EnumerateDirectories(Path))
                yield return new DirectoryContainerItem(new DriveContainer(child, Directory.GetLastWriteTime(child)));

            foreach (var file in Directory.EnumerateFiles(Path))
            {
                var fileInfo = new FileInfo(file);

                yield return new FileContainerItem(file, fileInfo.Length, fileInfo.LastWriteTime);
            }
        }

        public bool Equals([AllowNull] IContainer other)
        {
            if (other is DriveContainer otherDriveContainer)
                return Path == otherDriveContainer.Path;

            return false;
        }
    }
}
