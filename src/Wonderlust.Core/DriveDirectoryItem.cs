using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class DriveDirectoryItem : IItem
    {
        string path;

        public DriveDirectoryItem(string path)
        {
            this.path = path;
        }

        public string Name => Path.GetFileName(path);
    }
}
