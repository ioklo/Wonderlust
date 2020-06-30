using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;
using SystemPath = System.IO.Path;

namespace Wonderlust.Core
{
    class DirectoryContainerItem : IDirectoryContainerItem
    {
        DriveContainer container;

        public DirectoryContainerItem(DriveContainer container)
        {
            this.container = container;
        }

        // TODO: DriveItem이 아닌경우 Path를 사용하면 안된다
        public string Name => SystemPath.GetFileName(container.Path);
        public IContainer Container => container;

        public long? Size => null;

        public DateTime? DateTime => container.DateTime;
    }
}
