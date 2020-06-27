using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;
using SystemPath = System.IO.Path;

namespace Wonderlust.Core
{
    public class DriveContainer : IContainer
    {
        public string Path { get; }

        public DriveContainer(string path)
        {
            this.Path = path;
        }

        public IContainer? GetParent()
        {
            var dirName = SystemPath.GetDirectoryName(Path);
            if (dirName == null) return null;

            return new DriveContainer(dirName);            
        }

        public IEnumerable<IContainerItem> GetItems()
        {
            foreach (var child in Directory.EnumerateDirectories(Path))
                yield return new DirectoryContainerItem(child);

            foreach (var file in Directory.EnumerateFiles(Path))
                yield return new FileContainerItem(file);
        }

    }
}
