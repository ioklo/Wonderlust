using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class HistoryForwardInteractor
    {
        IWorkspace workspace;
        public HistoryForwardInteractor(IWorkspace workspace)
        {
            this.workspace = workspace;
        }

        public void Exec()
        {
            workspace.History.Forward();
        }
    }
}
