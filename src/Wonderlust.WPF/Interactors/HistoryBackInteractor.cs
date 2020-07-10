using System;
using System.Collections.Generic;
using System.Text;
using Wonderlust.Core.Abstractions;

namespace Wonderlust.WPF.Interactors
{
    class HistoryBackInteractor
    {
        IWorkspace workspace;

        public HistoryBackInteractor(IWorkspace workspace)
        {
            this.workspace = workspace;
        }

        public void Exec()
        {
            workspace.History.Back();
        }
    }
}
