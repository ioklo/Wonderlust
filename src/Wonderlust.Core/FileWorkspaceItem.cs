using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.Core
{
    public class FileWorkspaceItem : WorkspaceItem
    {
        FileContainerItem item;
        public override string DisplayName => item.Name;

        public override long? Size => item.Size;

        public override DateTime? DateTime => item.DateTime;

        public FileWorkspaceItem(Category category, FileContainerItem item)
            : base(category)
        {
            this.item = item;
        }

        public override void Exec()
        {
            try
            {
                var psi = new ProcessStartInfo();
                psi.FileName = item.Path;
                psi.UseShellExecute = true;

                Process.Start(psi);
            }
            catch
            {

            }
        }
    }
}
