using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;
using SystemPath = System.IO.Path;

namespace Wonderlust.Core
{
    public class FileContainerItem : IContainerItem
    {
        public string Path { get; }

        public string Name => SystemPath.GetFileName(Path);

        public long? Size { get; }

        public DateTime? DateTime { get; }

        public FileContainerItem(string path, long size, DateTime dateTime)
        {
            Path = path;
            Size = size;
            DateTime = dateTime;
        }


    }
}
