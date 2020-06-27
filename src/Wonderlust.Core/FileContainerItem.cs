using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class FileContainerItem : IContainerItem
    {
        string path;

        public string Name => Path.GetFileName(path);

        public FileContainerItem(string path)
        {
            this.path = path;
        }
    }
}
