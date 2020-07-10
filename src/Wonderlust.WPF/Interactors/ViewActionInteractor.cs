using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class ViewActionInteractor
    {
        IEnumerable<IWorkspaceItem> workspaceItems;

        public ViewActionInteractor(IEnumerable<IWorkspaceItem> workspaceItems)
        {
            this.workspaceItems = workspaceItems;
        }

        public void Exec()
        {
            var sb = new StringBuilder();
            bool bError = false, bFirst = true;
            foreach (var wi in workspaceItems)
            {
                if (wi.PhysicalPath == null)
                {
                    // TODO: 에러 전달 수단
                    bError = true;
                    break;
                }

                if (bFirst) bFirst = false;
                else sb.Append(' ');
                sb.Append($"\"{wi.PhysicalPath}\"");
            }

            if (!bError)
            {
                try
                {
                    var psi = new ProcessStartInfo();
                    psi.FileName = "code"; // TODO:                 
                    psi.Arguments = sb.ToString();
                    psi.UseShellExecute = true;

                    Process.Start(psi);
                }
                catch
                {

                }
            }
        }
    }
}
