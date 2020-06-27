using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;
using SystemPath = System.IO.Path;

namespace Wonderlust.Core
{
    class DirectoryContainerItem : IContainerItem
    {
        public string Path { get; }

        public DirectoryContainerItem(string path)
        {
            Path = path;
        }

        // TODO: DriveItem이 아닌경우 Path를 사용하면 안된다
        public string Name => SystemPath.GetFileName(Path);
    }
}
