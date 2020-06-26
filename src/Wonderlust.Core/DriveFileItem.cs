using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    class DriveFileItem : IItem
    {
        string path;

        public string Name => Path.GetFileName(path);

        public DriveFileItem(string path)
        {
            this.path = path;
        }
    }
}
